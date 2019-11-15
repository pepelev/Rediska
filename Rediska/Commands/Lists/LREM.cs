namespace Rediska.Commands.Lists
{
    using System;
    using System.Globalization;
    using Protocol;
    using Protocol.Visitors;

    public sealed class LREM : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("LREM");
        private readonly Key key;
        private readonly Count count;
        private readonly BulkString element;

        public LREM(Key key, Count count, BulkString element)
        {
            this.key = key;
            this.count = count;
            this.element = element;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString(),
            count.ToBulkString(),
            element
        );

        public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;

        public readonly struct Count : IEquatable<Count>
        {
            public static Count All { get; } = new Count(0);

            public static Count Left(long count)
            {
                if (count <= 0)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(count),
                        "Positive number expected, if you want to remove "
                        + "elements from right use " + nameof(Right)
                        + " method, constructor or implicit cast"
                    );
                }

                return new Count(count);
            }

            public static Count Right(long count)
            {
                if (count <= 0)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(count),
                        "Positive number expected, if you want to remove "
                        + "elements from left use " + nameof(Left)
                        + " method, constructor or implicit cast"
                    );
                }

                return new Count(checked(-count));
            }

            public Count(long value)
            {
                Value = value;
            }

            public long Value { get; }
            public bool Equals(Count other) => Value == other.Value;
            public override bool Equals(object obj) => obj is Count other && Equals(other);
            public override int GetHashCode() => Value.GetHashCode();

            public override string ToString()
            {
                if (Value == 0)
                {
                    return "All";
                }

                var count = Value.ToString(CultureInfo.InvariantCulture);
                return Value > 0
                    ? $"Remove first {count} elements moving from head (left) to tail (right)"
                    : $"Remove first {count} elements moving from tail (right) to head (left)";
            }

            public BulkString ToBulkString() => Value.ToBulkString();
            public static implicit operator Count(long value) => new Count(value);
            public static bool operator !=(Count left, Count right) => !left.Equals(right);
            public static bool operator ==(Count left, Count right) => left.Equals(right);
        }
    }
}