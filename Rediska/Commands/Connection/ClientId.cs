namespace Rediska.Commands.Connection
{
    using System;
    using System.Globalization;
    using Protocol;

    public readonly struct ClientId : IEquatable<ClientId>
    {
        public ClientId(long value)
        {
            Value = value;
        }

        public long Value { get; }
        public bool Equals(ClientId other) => Value == other.Value;
        public static bool operator ==(ClientId left, ClientId right) => left.Equals(right);
        public static bool operator !=(ClientId left, ClientId right) => !left.Equals(right);
        public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
        public BulkString ToBulkString(BulkStringFactory factory) => factory.Create(Value);
        public override bool Equals(object obj) => obj is ClientId other && Equals(other);
        public override int GetHashCode() => Value.GetHashCode();
    }
}