namespace Rediska.Commands.Streams
{
    using System;
    using Protocol;

    public readonly struct GroupName : IEquatable<GroupName>
    {
        private static readonly StringComparer equality = StringComparer.Ordinal;
        private readonly string value;

        public GroupName(string value)
        {
            this.value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Value => value ?? "";
        public bool Equals(GroupName other) => equality.Equals(Value, other.Value);
        public static bool operator ==(GroupName left, GroupName right) => left.Equals(right);
        public static implicit operator GroupName(string value) => new GroupName(value);
        public static bool operator !=(GroupName left, GroupName right) => !left.Equals(right);
        public BulkString ToBulkString(BulkStringFactory factory) => factory.Utf8(Value);
        public override bool Equals(object obj) => obj is GroupName other && Equals(other);
        public override int GetHashCode() => equality.GetHashCode(value);
        public override string ToString() => Value;
    }
}