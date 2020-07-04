namespace Rediska.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Protocol;

    public abstract class ScanCount
    {
        public static ScanCount Default { get; } = new DefaultScanCount();
        public static implicit operator ScanCount(uint count) => From(count);
        public static ScanCount From(long count) => new PlainScanCount(count);
        public abstract IEnumerable<BulkString> Arguments(BulkStringFactory factory);

        private sealed class DefaultScanCount : ScanCount
        {
            public override IEnumerable<BulkString> Arguments(BulkStringFactory factory) =>
                Enumerable.Empty<BulkString>();
        }

        private sealed class PlainScanCount : ScanCount
        {
            private static readonly PlainBulkString count = new PlainBulkString("COUNT");
            private readonly long value;

            public PlainScanCount(long value)
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value), value, "Must be positive");

                this.value = value;
            }

            public override IEnumerable<BulkString> Arguments(BulkStringFactory factory) => new[]
            {
                count,
                factory.Create(value)
            };
        }
    }
}