namespace Rediska.Commands.Sets
{
    using System;
    using System.Globalization;
    using Protocol;

    public readonly struct Count : IEquatable<Count>
    {
        public bool Equals(Count other) => Value == other.Value;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            return obj is Count other && Equals(other);
        }

        public override int GetHashCode() => Value.GetHashCode();
        public static bool operator ==(Count left, Count right) => left.Equals(right);
        public static bool operator !=(Count left, Count right) => !left.Equals(right);
        public static implicit operator Count(long value) => new Count(value);
        private const string ZeroCountIsMeaningless = "Zero count is meaningless";
        public long Value { get; }

        public Count(long value)
        {
            Value = value;
        }

        public static Count Distinct(long value)
        {
            if (value == 0)
                throw new ArgumentException(ZeroCountIsMeaningless, nameof(value));

            if (value < 0)
            {
                throw new ArgumentException(
                    "Positive number expected, if you want to allow Redis to"
                    + " return duplicates, use " + nameof(AllowRepeats) + " method",
                    nameof(value)
                );
            }

            return new Count(value);
        }

        public static Count AllowRepeats(long value)
        {
            if (value == 0)
                throw new ArgumentException(ZeroCountIsMeaningless, nameof(value));

            if (value < 0)
            {
                throw new ArgumentException(
                    "Positive number expected, if you want to deny Redis to"
                    + " return duplicates, use " + nameof(Distinct) + " method",
                    nameof(value)
                );
            }

            return new Count(-value);
        }

        public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
        public BulkString ToBulkString(BulkStringFactory factory) => factory.Create(Value);
    }
}