﻿namespace Rediska.Commands
{
    using System;
    using System.Globalization;
    using Protocol;

    public readonly struct UnixTimestamp : IEquatable<UnixTimestamp>, IComparable<UnixTimestamp>
    {
        internal static DateTime EpochStart => new DateTime(
            1970,
            1,
            1,
            0,
            0,
            0,
            0,
            DateTimeKind.Unspecified
        );

        public UnixTimestamp(long seconds)
        {
            if (seconds < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(seconds),
                    seconds,
                    "Second must be a nonnegative number"
                );
            }

            Seconds = seconds;
        }

        public long Seconds { get; }
        public int CompareTo(UnixTimestamp other) => Seconds.CompareTo(other.Seconds);
        public bool Equals(UnixTimestamp other) => Seconds == other.Seconds;
        public static bool operator ==(UnixTimestamp left, UnixTimestamp right) => left.Equals(right);
        public static bool operator >(UnixTimestamp left, UnixTimestamp right) => left.CompareTo(right) > 0;
        public static bool operator >=(UnixTimestamp left, UnixTimestamp right) => left.CompareTo(right) >= 0;
        public static bool operator !=(UnixTimestamp left, UnixTimestamp right) => !left.Equals(right);
        public static bool operator <(UnixTimestamp left, UnixTimestamp right) => left.CompareTo(right) < 0;
        public static bool operator <=(UnixTimestamp left, UnixTimestamp right) => left.CompareTo(right) <= 0;

        public DateTime ToDateTime(DateTimeKind kind)
        {
            var ticksSinceEpochStart = TimeSpan.TicksPerSecond * Seconds;
            var ticks = EpochStart.Ticks + ticksSinceEpochStart;
            return new DateTime(ticks, kind);
        }

        public BulkString ToBulkString() => Seconds.ToBulkString();

        public override string ToString()
        {
            var dateTime = ToDateTime(DateTimeKind.Utc);
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
        }

        public override bool Equals(object obj) => obj is UnixTimestamp other && Equals(other);
        public override int GetHashCode() => Seconds.GetHashCode();
    }
}