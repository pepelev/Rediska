﻿namespace Rediska.Commands
{
    using System;
    using System.Globalization;
    using Protocol;

    public readonly struct MillisecondsTimeout : IEquatable<MillisecondsTimeout>
    {
        public static MillisecondsTimeout Infinite { get; } = new MillisecondsTimeout(0);

        public MillisecondsTimeout(long milliseconds)
        {
            if (milliseconds < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(milliseconds),
                    "Milliseconds count must be non-negative"
                );
            }

            Milliseconds = milliseconds;
        }

        public long Milliseconds { get; }
        public bool Equals(MillisecondsTimeout other) => Milliseconds == other.Milliseconds;
        public static bool operator ==(MillisecondsTimeout left, MillisecondsTimeout right) => left.Equals(right);

        // todo remove implicit casts
        public static explicit operator MillisecondsTimeout(long value) => new MillisecondsTimeout(value);
        public static implicit operator MillisecondsTimeout(uint value) => new MillisecondsTimeout(value);
        public static bool operator !=(MillisecondsTimeout left, MillisecondsTimeout right) => !left.Equals(right);

        public static MillisecondsTimeout FromSeconds(long seconds)
        {
            if (seconds <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(seconds),
                    "Number of seconds must be positive, "
                    + "if you want to get infinite MillisecondsTimeout, "
                    + "use Infinite static property, "
                    + "constructor or cast"
                );
            }

            return new MillisecondsTimeout(seconds * 1_000);
        }

        public BulkString ToBulkString(BulkStringFactory factory) => factory.Create(Milliseconds);

        public override string ToString()
        {
            if (Milliseconds == 0)
            {
                return nameof(Infinite);
            }

            var duration = new TimeSpan(TimeSpan.TicksPerMillisecond * Milliseconds);
            return duration.ToString("g", CultureInfo.InvariantCulture);
        }

        public override bool Equals(object obj) => obj is MillisecondsTimeout other && Equals(other);
        public override int GetHashCode() => Milliseconds.GetHashCode();
    }
}