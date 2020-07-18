namespace Rediska.Commands.Streams
{
    using System;

    public sealed class Interval
    {
        public static Interval Whole { get; } = new Interval(Id.Minimum, Id.Maximum);

        public Interval(Id startInclusive, Id endInclusive)
        {
            if (endInclusive < startInclusive)
            {
                throw new ArgumentException(
                    $"Interval must be non-void. Interval {startInclusive}..{endInclusive} is void"
                );
            }

            StartInclusive = startInclusive;
            EndInclusive = endInclusive;
        }

        public Id StartInclusive { get; }
        public Id EndInclusive { get; }
        public override string ToString() => $"[{StartInclusive}..{EndInclusive}]";
        public static Interval FromInclusive(Id startInclusive) => new Interval(startInclusive, Id.Maximum);
        public static Interval FromExclusive(Id startExclusive) => FromInclusive(startExclusive.Next());
        public static Interval ToInclusive(Id endInclusive) => new Interval(Id.Minimum, endInclusive);
        public static Interval ToExclusive(Id startExclusive) => ToInclusive(startExclusive.Previous());
    }
}