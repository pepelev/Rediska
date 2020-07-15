namespace Rediska.Commands
{
    using System;
    using System.Globalization;
    using Protocol;

    public readonly struct DatabaseNumber : IEquatable<DatabaseNumber>
    {
        public DatabaseNumber(long value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), value, "Must be non-negative");

            Value = value;
        }

        public long Value { get; }
        public bool Equals(DatabaseNumber other) => Value == other.Value;
        public static bool operator ==(DatabaseNumber left, DatabaseNumber right) => left.Equals(right);
        public static bool operator !=(DatabaseNumber left, DatabaseNumber right) => !left.Equals(right);
        public override bool Equals(object obj) => obj is DatabaseNumber other && Equals(other);
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
        public BulkString ToBulkString(BulkStringFactory factory) => factory.Create(Value);
    }
}