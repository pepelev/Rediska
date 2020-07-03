namespace Rediska.Commands.Server
{
    using System;

    public sealed partial class CommandDescription
    {
        public readonly struct ArityDescription : IEquatable<ArityDescription>
        {
            public ArityDescription(long rawValue)
            {
                RawValue = rawValue;
            }

            public long RawValue { get; }

            public ArityMode Mode => RawValue >= 0
                ? ArityMode.FixedArgumentCount
                : ArityMode.MinimumArgumentCount;

            public long Count => Math.Abs(RawValue);

            public override string ToString() => Mode switch
            {
                ArityMode.FixedArgumentCount => $"Fixed {Count}",
                _ => $"Minimum {Count}"
            };

            public bool Equals(ArityDescription other) => RawValue == other.RawValue;
            public override bool Equals(object obj) => obj is ArityDescription other && Equals(other);
            public override int GetHashCode() => RawValue.GetHashCode();
            public static bool operator ==(ArityDescription left, ArityDescription right) => left.Equals(right);
            public static bool operator !=(ArityDescription left, ArityDescription right) => !left.Equals(right);
        }
    }
}