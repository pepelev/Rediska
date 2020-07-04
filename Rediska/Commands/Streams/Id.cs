namespace Rediska.Commands.Streams
{
    using System;
    using System.Diagnostics.Contracts;

    public readonly struct Id : IEquatable<Id>, IComparable<Id>
    {
        public Id(ulong high, ulong low)
        {
            High = high;
            Low = low;
        }

        public ulong High { get; }
        public ulong Low { get; }
        public UnixMillisecondsTimestamp Timestamp => new UnixMillisecondsTimestamp(checked((long)High));
        public ulong SequenceNumber => Low;

        [Pure]
        public Id Next()
        {
            return Low < ulong.MaxValue
                ? new Id(High, Low + 1)
                : MinFor(High + 1);
        }

        public bool Equals(Id other) => High == other.High && Low == other.Low;
        public override bool Equals(object obj) => obj is Id other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                return (High.GetHashCode() * 397) ^ Low.GetHashCode();
            }
        }

        public int CompareTo(Id other)
        {
            var highComparison = High.CompareTo(other.High);
            if (highComparison != 0)
                return highComparison;

            return Low.CompareTo(other.Low);
        }

        public static bool operator ==(Id left, Id right) => left.Equals(right);
        public static bool operator !=(Id left, Id right) => !left.Equals(right);
        public static bool operator <(Id left, Id right) => left.CompareTo(right) < 0;
        public static bool operator >(Id left, Id right) => left.CompareTo(right) > 0;
        public static bool operator <=(Id left, Id right) => left.CompareTo(right) <= 0;
        public static bool operator >=(Id left, Id right) => left.CompareTo(right) >= 0;
        public override string ToString() => $"{High}-{Low}";

        public static Id MinFor(ulong high) => new Id(high, ulong.MinValue);
        public static Id MaxFor(ulong high) => new Id(high, ulong.MaxValue);

        public static Id MinFor(DateTime dateTime) => MinFor(
            ToUnixMillisecondsTimestamp(dateTime)
        );

        public static Id MaxFor(DateTime dateTime) => MaxFor(
            ToUnixMillisecondsTimestamp(dateTime)
        );

        private static ulong ToUnixMillisecondsTimestamp(DateTime dateTime) =>
            checked((ulong) (dateTime.Ticks / TimeSpan.TicksPerMillisecond));
    }
}