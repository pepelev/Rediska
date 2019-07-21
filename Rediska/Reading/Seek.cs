using System;
using System.IO;
using System.Threading;

namespace Rediska.Reading
{
    public sealed class Seek : State
    {
        private static readonly Seek[] cache = new Seek[256];

        private readonly long seekLeft;

        public Seek(long seekLeft)
        {
            this.seekLeft = seekLeft;
        }

        public override State Transit(Stream stream)
        {
            var size = stream.Length - stream.Position;
            if (seekLeft <= size)
            {
                stream.Seek(seekLeft, SeekOrigin.Current);
                return End.Singleton;
            }

            stream.Seek(0, SeekOrigin.End);
            return Get(seekLeft - size);
        }

        public static State Get(long size)
        {
            if (size == 0)
                return End.Singleton;

            if (size < 0)
                throw new ArgumentException("Size must be nonnegative", nameof(size));

            if (size > 256)
                return new Seek(size);

            var cached = Volatile.Read(ref cache[size - 1]);
            if (cached != null)
                return cached;

            var result = new Seek(size);
            Volatile.Write(ref cache[size - 1], result);
            return result;
        }

        public override bool IsTerminal => false;
    }
}