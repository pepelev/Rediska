namespace Rediska.Commands.SortedSets
{
    using System;

    public sealed class LexInterval
    {
        public static LexInterval Universal { get; } = new LexInterval(
            LexBound.NegativeInfinity,
            LexBound.PositiveInfinity
        );

        public LexInterval(LexBound min, LexBound max)
        {
            if (LexBound.IsVoid(min, max))
                throw new ArgumentException($"Interval must be non-void. Interval {min}..{max} is void");

            Min = min;
            Max = max;
        }

        public LexBound Min { get; }
        public LexBound Max { get; }
        public override string ToString() => $"{Min}..{Max}";
        public static LexInterval From(LexBound min) => new LexInterval(min, LexBound.PositiveInfinity);
        public static LexInterval To(LexBound max) => new LexInterval(LexBound.NegativeInfinity, max);
    }
}