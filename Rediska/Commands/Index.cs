namespace Rediska.Commands
{
    using System;
    using System.Globalization;
    using Protocol;

    public readonly struct Index : IEquatable<Index>
    {
        public static Index Start => FromStart(0);
        public static Index End => FromEnd(0);

        public Index(long value)
        {
            Value = value;
        }

        public long Value { get; }
        public bool IsFromStart => Value >= 0;
        public bool IsFromEnd => !IsFromStart;
        public bool Equals(Index other) => Value == other.Value;
        public static bool operator ==(Index left, Index right) => left.Equals(right);
        public static implicit operator Index(long value) => new Index(value);
        public static bool operator !=(Index left, Index right) => !left.Equals(right);

        public static Index FromStart(long index)
        {
            if (index < 0)
            {
                throw new ArgumentException(
                    "Nonnegative number expected, if you want to create an index "
                    + "starting at the end of the list, use " + nameof(FromEnd)
                    + " method, constructor or implicit cast from System.Int64",
                    nameof(index)
                );
            }

            return new Index(index);
        }

        public static Index FromEnd(long index)
        {
            if (index < 0)
            {
                throw new ArgumentException(
                    "Nonnegative number expected, if you want to create an index "
                    + "starting at the beginning of the list, use " + nameof(FromStart)
                    + " method, constructor or implicit cast from System.Int64",
                    nameof(index)
                );
            }

            return new Index(checked(-index - 1));
        }

        public override bool Equals(object obj) => obj is Index other && Equals(other);
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
        public BulkString ToBulkString(BulkStringFactory factory) => factory.Create(Value);
    }
}