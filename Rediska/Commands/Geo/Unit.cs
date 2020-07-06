namespace Rediska.Commands.Geo
{
    using System;
    using Protocol;

    public sealed class Unit : IEquatable<Unit>
    {
        public static Unit Meter { get; } = new Unit(new PlainBulkString("m"));
        public static Unit Kilometer { get; } = new Unit(new PlainBulkString("km"));
        public static Unit Mile { get; } = new Unit(new PlainBulkString("mi"));
        public static Unit Feet { get; } = new Unit(new PlainBulkString("ft"));
        private readonly BulkString content;

        public Unit(BulkString content)
        {
            this.content = content;
        }

        public bool Equals(Unit other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (ReferenceEquals(null, other))
                return false;

            return content.Equals(other.content);
        }

        public static bool operator ==(Unit left, Unit right) => Equals(left, right);
        public static bool operator !=(Unit left, Unit right) => !Equals(left, right);
        public override bool Equals(object obj) => ReferenceEquals(this, obj) || obj is Unit other && Equals(other);
        public override int GetHashCode() => content.GetHashCode();
        public BulkString ToBulkString() => content;
        public override string ToString() => content.ToString();
    }
}