namespace Rediska.Commands
{
    using System;
    using System.Globalization;
    using Protocol;

    public readonly struct MillisecondsTimeout : IEquatable<MillisecondsTimeout>
    {
        public static MillisecondsTimeout Infinite { get; } = new MillisecondsTimeout(0);

        public MillisecondsTimeout(long milliseconds)
        {
            if (milliseconds <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(milliseconds),
                    "Seconds count must be nonnegative"
                );
            }

            Milliseconds = milliseconds;
        }

        public long Milliseconds { get; }

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

        public BulkString ToBulkString() => Milliseconds.ToBulkString();

        public override string ToString()
        {
            if (Milliseconds == 0)
            {
                return nameof(Infinite);
            }

            var duration = new TimeSpan(TimeSpan.TicksPerMillisecond * Milliseconds);
            return duration.ToString("g", CultureInfo.InvariantCulture);
        }

        // todo remove implicit casts
        public static explicit operator MillisecondsTimeout(long value) => new MillisecondsTimeout(value);
        public static implicit operator MillisecondsTimeout(uint value) => new MillisecondsTimeout(value);
        public bool Equals(MillisecondsTimeout other) => Milliseconds == other.Milliseconds;
        public override bool Equals(object obj) => obj is MillisecondsTimeout other && Equals(other);
        public override int GetHashCode() => Milliseconds.GetHashCode();
        public static bool operator ==(MillisecondsTimeout left, MillisecondsTimeout right) => left.Equals(right);
        public static bool operator !=(MillisecondsTimeout left, MillisecondsTimeout right) => !left.Equals(right);
    }
}