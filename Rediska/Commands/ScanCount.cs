using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Rediska.Protocol;
using Rediska.Utils;

namespace Rediska.Commands
{
    public abstract class ScanCount : IReadOnlyList<DataType>
    {
        public static ScanCount None { get; } = new NoneScanCount();
        public static ScanCount From(long count) => new PlainScanCount(count);
        public static implicit operator ScanCount(long count) => From(count);

        public abstract IEnumerator<DataType> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public abstract DataType this[int index] { get; }
        public abstract int Count { get; }

        private sealed class NoneScanCount : ScanCount
        {
            public override IEnumerator<DataType> GetEnumerator() => EmptyEnumerator<DataType>.Singleton;
            public override DataType this[int index] => throw new IndexOutOfRangeException();
            public override int Count => 0;
        }

        private sealed class PlainScanCount : ScanCount
        {
            private static readonly PlainBulkString keyword = new PlainBulkString("COUNT");

            private readonly long value;

            public PlainScanCount(long value)
            {
                this.value = value;
            }

            public override int Count => 2;
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