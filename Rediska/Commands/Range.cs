namespace Rediska.Commands
{
    using System;

    public readonly struct Range : IEquatable<Range>
    {
        public static Range Whole => new Range(Index.FromStart(0), Index.FromEnd(0));

        public Range(Index startInclusive, Index endInclusive)
        {
            const string parameterNames = nameof(startInclusive) + "|" + nameof(endInclusive);
            switch (startInclusive, endInclusive)
            {
                case (
                    { IsFromStart: true, Value: var start },
                    { IsFromStart: true, Value: var end }
                    ) when start > end:
                    throw new ArgumentException(Message(), parameterNames);

                case (
                    { IsFromStart: false, Value: var start },
                    { IsFromStart: false, Value: var end }
                    ) when start > end:
                    throw new ArgumentException(Message(), parameterNames);
            }

            string Message() => $"Range [{startInclusive}..{endInclusive}] can't contain any element";

            StartInclusive = startInclusive;
            EndInclusive = endInclusive;
        }

        public Index StartInclusive { get; }
        public Index EndInclusive { get; }

        public bool Equals(Range other)
        {
            return StartInclusive.Equals(other.StartInclusive) && EndInclusive.Equals(other.EndInclusive);
        }

        public static bool operator ==(Range left, Range right) => left.Equals(right);
        public static bool operator !=(Range left, Range right) => !left.Equals(right);
        public override bool Equals(object obj) => obj is Range other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                return (StartInclusive.GetHashCode() * 397) ^ EndInclusive.GetHashCode();
            }
        }

        public override string ToString() => $"[{StartInclusive}..{EndInclusive}]";
    }
}