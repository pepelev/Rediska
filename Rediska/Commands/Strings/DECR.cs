namespace Rediska.Commands.Strings
{
    using Protocol;
    using Protocol.Visitors;

    public sealed class DECR : Command<DECR.Response>
    {
        private static readonly PlainBulkString name = new PlainBulkString("DECR");

        private static readonly Visitor<Response> responseStructure = IntegerExpectation.Singleton
            .Then(valueAfterDecrement => new Response(valueAfterDecrement));

        private readonly Key key;

        public DECR(Key key)
        {
            this.key = key;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString()
        );

        public override Visitor<Response> ResponseStructure => responseStructure;

        public readonly struct Response
        {
            public Response(long valueAfterDecrement)
            {
                ValueAfterDecrement = valueAfterDecrement;
            }

            public long ValueBeforeDecrement => ValueAfterDecrement - 1;
            public long ValueAfterDecrement { get; }
        }
    }
}