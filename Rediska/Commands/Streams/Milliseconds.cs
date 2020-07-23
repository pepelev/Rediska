namespace Rediska.Commands.Streams
{
    using System;
    using System.Globalization;
    using Protocol;

    public readonly struct Milliseconds : IEquatable<Milliseconds>, IComparable<Milliseconds>
    {
        public Milliseconds(long value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), value, "Must be non-negative");

            Value = value;
        }

        public long Value { get; }
        public int CompareTo(Milliseconds other) => Value.CompareTo(other.Value);
        public bool Equals(Milliseconds other) => Value == other.Value;
        public static bool operator ==(Milliseconds left, Milliseconds right) => left.Equals(right);
        public static bool operator >(Milliseconds left, Milliseconds right) => left.CompareTo(right) > 0;
        public static bool operator >=(Milliseconds left, Milliseconds right) => left.CompareTo(right) >= 0;
        public static bool operator !=(Milliseconds left, Milliseconds right) => !left.Equals(right);
        public static bool operator <(Milliseconds left, Milliseconds right) => left.CompareTo(right) < 0;
        public static bool operator <=(Milliseconds left, Milliseconds right) => left.CompareTo(right) <= 0;
        public BulkString ToBulkString(BulkStringFactory factory) => factory.Create(Value);
        public TimeSpan ToTimeSpan() => new TimeSpan(TimeSpan.TicksPerMillisecond * Value);
        public override bool Equals(object obj) => obj is Milliseconds other && Equals(other);
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => ToTimeSpan().ToString("g", CultureInfo.InvariantCulture);
    }
}