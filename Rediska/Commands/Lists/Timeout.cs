namespace Rediska.Commands.Lists
{
    using System;
    using System.Globalization;
    using Protocol;

    public readonly struct Timeout : IEquatable<Timeout>
    {
        public static Timeout Infinite { get; } = new Timeout(0);

        public Timeout(long seconds)
        {
            if (seconds <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(seconds),
                    "Seconds count must be nonnegative"
                );
            }

            Seconds = seconds;
        }

        public long Seconds { get; }

        public static Timeout FromSeconds(long seconds)
        {
            if (seconds <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(seconds),
                    "Number of seconds must be positive, "
                    + "if you want to get infinite Timeout, "
                    + "use Infinite static property, "
                    + "constructor or cast"
                );
            }

            return new Timeout(seconds);
        }

        public BulkString ToBulkString() => Seconds.ToBulkString();

        public override string ToString()
        {
            if (Seconds == 0)
            {
                return nameof(Infinite);
            }

            var duration = new TimeSpan(TimeSpan.TicksPerSecond * Seconds);
            return duration.ToString("g", CultureInfo.InvariantCulture);
        }

        public static explicit operator Timeout(long value) => new Timeout(value);
        public static implicit operator Timeout(uint value) => new Timeout(value);
        public bool Equals(Timeout other) => Seconds == other.Seconds;
        public override bool Equals(object obj) => obj is Timeout other && Equals(other);
        public override int GetHashCode() => Seconds.GetHashCode();
        public static bool operator ==(Timeout left, Timeout right) => left.Equals(right);
        public static bool operator !=(Timeout left, Timeout right) => !left.Equals(right);
    }
}