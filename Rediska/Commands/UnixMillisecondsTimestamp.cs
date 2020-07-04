namespace Rediska.Commands
{
    using System;
    using System.Globalization;
    using Protocol;

    public readonly struct UnixMillisecondsTimestamp : IEquatable<UnixMillisecondsTimestamp>, IComparable<UnixMillisecondsTimestamp>
    {
        public UnixMillisecondsTimestamp(long milliseconds)
        {
            if (milliseconds < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(milliseconds),
                    milliseconds,
                    "Milliseconds must be a nonnegative number"
                );
            }

            Milliseconds = milliseconds;
        }

        public long Milliseconds { get; }

        public DateTime ToDateTime(DateTimeKind kind)
        {
            var ticksSinceEpochStart = TimeSpan.TicksPerSecond * Milliseconds;
            var ticks = UnixTimestamp.EpochStart.Ticks + ticksSinceEpochStart;
            return new DateTime(ticks, kind);
        }

        public BulkString ToBulkString() => Milliseconds.ToBulkString();

        public override string ToString()
        {
            var dateTime = ToDateTime(DateTimeKind.Utc);
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
        }

        public bool Equals(UnixMillisecondsTimestamp other) => Milliseconds == other.Milliseconds;
        public override bool Equals(object obj) => obj is UnixMillisecondsTimestamp other && Equals(other);
        public override int GetHashCode() => Milliseconds.GetHashCode();

        public static bool operator ==(UnixMillisecondsTimestamp left, UnixMillisecondsTimestamp right) =>
            left.Equals(right);

        public static bool operator !=(UnixMillisecondsTimestamp left, UnixMillisecondsTimestamp right) =>
            !left.Equals(right);

        public int CompareTo(UnixMillisecondsTimestamp other) => Milliseconds.CompareTo(other.Milliseconds);

        public static bool operator <(UnixMillisecondsTimestamp left, UnixMillisecondsTimestamp right) =>
            left.CompareTo(right) < 0;

        public static bool operator >(UnixMillisecondsTimestamp left, UnixMillisecondsTimestamp right) =>
            left.CompareTo(right) > 0;

        public static bool operator <=(UnixMillisecondsTimestamp left, UnixMillisecondsTimestamp right) =>
            left.CompareTo(right) <= 0;

        public static bool operator >=(UnixMillisecondsTimestamp left, UnixMillisecondsTimestamp right) =>
            left.CompareTo(right) >= 0;
    }
}