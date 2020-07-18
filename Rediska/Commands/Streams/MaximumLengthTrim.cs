namespace Rediska.Commands.Streams
{
    using System;
    using Auxiliary;
    using Protocol;

    public readonly struct MaximumLengthTrim
    {
        private static readonly PlainBulkString maxlen = new PlainBulkString("MAXLEN");
        private static readonly PlainBulkString approximately = new PlainBulkString("~");
        private readonly Mode mode;

        private MaximumLengthTrim(Mode mode, long count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Must be non-negative");

            this.mode = mode;
            Count = count;
        }

        public long Count { get; }

        public BulkString[] Arguments(BulkStringFactory factory)
        {
            return mode switch
            {
                Mode.Strict => new[] {maxlen, factory.Create(Count)},
                _ => new[] {maxlen, approximately, factory.Create(Count)}
            };
        }

        public override string ToString() => new PlainCommand(Arguments(BulkStringFactory.Plain)).ToString();
        public static MaximumLengthTrim Exact(long count) => new MaximumLengthTrim(Mode.Strict, count);
        public static MaximumLengthTrim Roughly(long count) => new MaximumLengthTrim(Mode.Lax, count);

        private enum Mode : byte
        {
            Lax = 0,
            Strict = 1
        }
    }
}