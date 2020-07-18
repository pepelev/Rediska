namespace Rediska.Commands.Streams
{
    using System;
    using Protocol;

    public sealed class Limit : Count
    {
        private static readonly PlainBulkString argument = new PlainBulkString("COUNT");
        private readonly long count;

        public Limit(long count)
        {
            if (count < 1)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Must be positive");

            this.count = count;
        }

        public override BulkString[] Arguments(BulkStringFactory factory) => new[]
        {
            argument,
            factory.Create(count)
        };
    }
}