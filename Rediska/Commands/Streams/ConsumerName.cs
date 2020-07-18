namespace Rediska.Commands.Streams
{
    using System;
    using Protocol;

    public readonly struct Consumer : IEquatable<Consumer>
    {
        private static readonly StringComparer equality = StringComparer.Ordinal;
        private readonly string value;

        public Consumer(string value)
        {
            this.value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Value => value ?? "";
        public bool Equals(Consumer other) => equality.Equals(Value, other.Value);
        public static bool operator ==(Consumer left, Consumer right) => left.Equals(right);
        public static implicit operator Consumer(string value) => new Consumer(value);
        public static bool operator !=(Consumer left, Consumer right) => !left.Equals(right);
        public BulkString ToBulkString(BulkStringFactory factory) => factory.Utf8(Value);
        public override bool Equals(object obj) => obj is Consumer other && Equals(other);
        public override int GetHashCode() => equality.GetHashCode(value);
        public override string ToString() => Value;
    }
}