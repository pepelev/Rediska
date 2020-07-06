namespace Rediska.Commands.SortedSets
{
    using System;

    public sealed class ScoreInterval
    {
        public static ScoreInterval Universal { get; } = new ScoreInterval(
            ScoreBound.NegativeInfinity,
            ScoreBound.PositiveInfinity
        );

        public ScoreInterval(ScoreBound min, ScoreBound max)
        {
            if (ScoreBound.IsVoid(min, max))
                throw new ArgumentException($"Interval must be non-void. Interval {min}..{max} is void");

            Min = min;
            Max = max;
        }

        public ScoreBound Min { get; }
        public ScoreBound Max { get; }
        public override string ToString() => $"{Min}..{Max}";
        public static ScoreInterval From(ScoreBound min) => new ScoreInterval(min, ScoreBound.PositiveInfinity);
        public static ScoreInterval To(ScoreBound max) => new ScoreInterval(ScoreBound.NegativeInfinity, max);
    }
}