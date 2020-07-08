namespace Rediska.Commands.SortedSets
{
    using System;
    using System.Globalization;
    using Protocol;

    public readonly struct LexBound
    {
        private readonly Kind kind;
        private readonly BulkString value;
    }

    public readonly struct ScoreBound
    {
        public static ScoreBound PositiveInfinity => new ScoreBound(Kind.PositiveInfinity, 0.0);
        public static ScoreBound NegativeInfinity => new ScoreBound(Kind.NegativeInfinity, 0.0);
        private readonly Kind kind;
        private readonly double value;

        private ScoreBound(Kind kind, double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Must be regular double", nameof(value));

            this.kind = kind;
            this.value = value;
        }

        public BulkString ToBulkString(BulkStringFactory factory) => kind switch
        {
            Kind.NegativeInfinity => Bounds.NegativeInfinity,
            Kind.PositiveInfinity => Bounds.PositiveInfinity,
            Kind.Inclusive => factory.Create(value),
            _ => factory.Utf8(Bounds.ExclusiveSign + value.ToString(CultureInfo.InvariantCulture))
        };

        public override string ToString() => ToBulkString(BulkStringFactory.Plain).ToString();
        public static ScoreBound Inclusive(double value) => new ScoreBound(Kind.Inclusive, value);
        public static ScoreBound Exclusive(double value) => new ScoreBound(Kind.Exclusive, value);

        public static bool IsVoid(in ScoreBound min, in ScoreBound max) => (min.kind, max.kind) switch
        {
            (Kind.NegativeInfinity, _) => false,
            (_, Kind.PositiveInfinity) => false,
            (Kind.Inclusive, Kind.Inclusive) => min.value <= max.value,
            (Kind.Exclusive, Kind.Exclusive) => min.value < max.value,
            (Kind.Exclusive, Kind.Inclusive) => min.value < max.value,
            (Kind.Inclusive, Kind.Exclusive) => min.value < max.value,
            _ => true
        };
    }
}