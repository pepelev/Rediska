namespace Rediska.Commands.Strings
{
    using Protocol;
    using Protocol.Visitors;

    public sealed class INCRBY : Command<INCRBY.Response>
    {
        private static readonly PlainBulkString name = new PlainBulkString("INCRBY");
        private readonly Key key;
        private readonly long increment;

        public INCRBY(Key key, long increment)
        {
            this.key = key;
            this.increment = increment;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString(),
            increment.ToBulkString()
        );

        public override Visitor<Response> ResponseStructure => IntegerExpectation.Singleton
            .Then(valueAfterIncrement => new Response(increment, valueAfterIncrement));

        public readonly struct Response
        {
            public Response(long increment, long valueAfterIncrement)
            {
                Increment = increment;
                ValueAfterIncrement = valueAfterIncrement;
            }

            public long Increment { get; }
            public long ValueBeforeIncrement => ValueAfterIncrement - Increment;
            public long ValueAfterIncrement { get; }
        }
    }
}