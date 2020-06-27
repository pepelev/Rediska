namespace Rediska.Commands.Geo
{
    using System;
    using Protocol;

    public readonly struct Unit : IEquatable<Unit>
    {
        private readonly BulkString content;

        public Unit(BulkString content)
        {
            this.content = content;
        }

        public bool Equals(Unit other) => Content.Equals(other.Content);
        public static Unit Meter { get; } = new Unit(new PlainBulkString("m"));
        public static Unit Kilometer { get; } = new Unit(new PlainBulkString("km"));
        public static Unit Mile { get; } = new Unit(new PlainBulkString("mi"));
        public static Unit Feet { get; } = new Unit(new PlainBulkString("ft"));
        public override bool Equals(object obj) => obj is Unit other && Equals(other);
        public static bool operator ==(Unit left, Unit right) => left.Equals(right);
        public static bool operator !=(Unit left, Unit right) => !left.Equals(right);
        public override int GetHashCode() => Content.GetHashCode();
        public BulkString ToBulkString() => Content;
        public override string ToString() => Content.ToString();
        private BulkString Content => content ?? Meter.Content;
    }
}