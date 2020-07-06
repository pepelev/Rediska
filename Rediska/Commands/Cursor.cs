namespace Rediska.Commands
{
    using System;
    using Protocol;

    public readonly struct Cursor : IEquatable<Cursor>, IComparable<Cursor>
    {
        public static Cursor Start => new Cursor(0);

        public Cursor(long value)
        {
            Value = value;
        }

        public long Value { get; }
        public int CompareTo(Cursor other) => Value.CompareTo(other.Value);
        public bool Equals(Cursor other) => Value == other.Value;
        public static bool operator ==(Cursor left, Cursor right) => left.Equals(right);
        public static bool operator !=(Cursor left, Cursor right) => !left.Equals(right);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            return obj is Cursor other && Equals(other);
        }

        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();
        public BulkString ToBulkString(BulkStringFactory factory) => factory.Create(Value);
    }
}