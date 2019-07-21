using System;
using System.IO;
using System.Threading;

namespace Rediska.Reading
{
    public sealed class Array : State
    {
        private static readonly Array[] cache = new Array[1024];

        private readonly long itemsLeft;
        private readonly State itemState;

        public Array(long itemsLeft, State itemState)
        {
            this.itemsLeft = itemsLeft;
            this.itemState = itemState;
        }

        public override State Transit(Stream stream)
        {
            if (itemState.IsTerminal)
            {
                return itemsLeft == 1
                    ? End.Singleton
                    : Get(itemsLeft - 1);
            }

            var newItemState = itemState.Transit(stream);
            return new Array(itemsLeft, newItemState);
        }

        public static State Get(long length)
        {
            if (length == 0)
                return End.Singleton;

            if (length < 0)
                throw new ArgumentException("Length must be positive");

            var index = length - 1;
            if (index >= cache.Length)
                return new Array(length, Start.Singleton);

            var cached = Volatile.Read(ref cache[index]);
            if (cached != null)
                return cached;

            var result = new Array(length, Start.Singleton);
            Volatile.Write(ref cache[index], result);
            return result;
        }

        public override bool IsTerminal => itemsLeft == 1 && itemState.IsTerminal;
    }

    public sealed class ArrayLength : State
    {
        private readonly long partiallyParsedLength;

        public ArrayLength(long partiallyParsedLength)
        {
            this.partiallyParsedLength = partiallyParsedLength;
        }

        public override State Transit(Stream stream)
        {
            var length = partiallyParsedLength;
            while (true)
            {
                var @byte = stream.ReadByte();
                if (@byte == -1)
                {
                    return length == partiallyParsedLength
                        ? this
                        : new ArrayLength(length);
                }

                if (@byte == '\r')
                {
                    // \n (length items)
                    return new Serial(
                        Seek.Get(1),
                        Array.Get(length)
                    );
                }

                length = length * 10 + @byte - '0';
            }
        }

        public override bool IsTerminal => false;
    }

    public sealed class Serial : State
    {
        public Serial(State first, State second)
        {
            this.first = first;
            this.second = second;
        }

        private readonly State first;
        private readonly State second;

        public override State Transit(Stream stream)
        {
            if (first.IsTerminal)
                return second;

            var newState = first.Transit(stream);
            if (newState.IsTerminal)
                return second;

            return new Serial(newState, second);
        }

        public override bool IsTerminal => false;
    }
}