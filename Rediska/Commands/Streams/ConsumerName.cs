namespace Rediska.Commands.Streams
{
    using System;
    using Protocol;

    public readonly struct ConsumerName : IEquatable<ConsumerName>
    {
        private static readonly StringComparer equality = StringComparer.Ordinal;
        private readonly string value;

        public ConsumerName(string value)
        {
            this.value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Value => value ?? "";
        public bool Equals(ConsumerName other) => equality.Equals(Value, other.Value);
        public static bool operator ==(ConsumerName left, ConsumerName right) => left.Equals(right);
        public static implicit operator ConsumerName(string value) => new ConsumerName(value);
        public static bool operator !=(ConsumerName left, ConsumerName right) => !left.Equals(right);
        public BulkString ToBulkString(BulkStringFactory factory) => factory.Utf8(Value);
        public override bool Equals(object obj) => obj is ConsumerName other && Equals(other);
        public override int GetHashCode() => equality.GetHashCode(value);
        public override string ToString() => Value;
    }
}