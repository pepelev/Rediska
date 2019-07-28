using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Rediska.Protocol;
using Rediska.Utils;

namespace Rediska.Commands
{
    public abstract class Count : IReadOnlyList<DataType>
    {
        public static Count None { get; } = new NoneCount();
        public static Count From(long count) => new PlainCount(count);
        public static implicit operator Count(long count) => From(count);

        public abstract int Length { get; }
        public abstract IEnumerator<DataType> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public abstract DataType this[int index] { get; }
        int IReadOnlyCollection<DataType>.Count => Length;

        private sealed class NoneCount : Count
        {
            public override int Length => 0;
            public override IEnumerator<DataType> GetEnumerator() => EmptyEnumerator<DataType>.Singleton;
            public override DataType this[int index] => throw new IndexOutOfRangeException();
        }

        private sealed class PlainCount : Count
        {
            private static readonly PlainBulkString keyword = new PlainBulkString("COUNT");

            private readonly long value;

            public PlainCount(long value)
            {
                this.value = value;
            }

            public override int Length => 2;
            public override IEnumerator<DataType> GetEnumerator()
            {
                yield return keyword;
                yield return Value;
            }

            public override DataType this[int index]
            {
                get
                {
                    switch (index)
                    {
                        case 0:
                            return keyword;
                        case 1:
                            return Value;
                        default:
                            throw new IndexOutOfRangeException();
                    }
                }
            }

            private PlainBulkString Value => new PlainBulkString(value.ToString(CultureInfo.InvariantCulture));
        }
    }
}