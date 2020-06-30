namespace Rediska.Commands.Strings
{
    using System;
    using System.Globalization;
    using Protocol;

    public readonly struct Offset
    {
        public Offset(OffsetUnit unit, long count)
        {
            if (unit != OffsetUnit.Bit && unit != OffsetUnit.Integer)
                throw new ArgumentException("Must be bits or integer", nameof(unit));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Must be positive");

            Unit = unit;
            Count = count;
        }

        public OffsetUnit Unit { get; }
        public long Count { get; }

        public override string ToString()
        {
            return Unit switch
            {
                OffsetUnit.Bit => Count.ToString(CultureInfo.InvariantCulture),
                OffsetUnit.Integer => $"#{Count.ToString(CultureInfo.InvariantCulture)}",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public BulkString ToBulkString() => new PlainBulkString(ToString());

        public static Offset Bits(long count) => new Offset(OffsetUnit.Bit, count);
        public static Offset Integers(long count) => new Offset(OffsetUnit.Integer, count);
        public static Offset Zero => Bits(0);
    }
}