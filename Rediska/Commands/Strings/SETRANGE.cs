namespace Rediska.Commands.Strings
{
    using Protocol;
    using Protocol.Visitors;

    public sealed class SETRANGE : Command<SETRANGE.Response>
    {
        private static readonly PlainBulkString name = new PlainBulkString("SETRANGE");

        private static readonly Visitor<Response> responseStructure = IntegerExpectation.Singleton
            .Then(stringLengthAfterOperation => new Response(stringLengthAfterOperation));

        private readonly Key key;
        private readonly long offset;
        private readonly BulkString value;

        public SETRANGE(Key key, long offset, BulkString value)
        {
            this.key = key;
            this.offset = offset;
            this.value = value;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString(),
            offset.ToBulkString(),
            value
        );

        public override Visitor<Response> ResponseStructure => responseStructure;

        public readonly struct Response
        {
            public Response(long stringLengthAfterOperation)
            {
                StringLengthAfterOperation = stringLengthAfterOperation;
            }

            public long StringLengthAfterOperation { get; }
        }
    }
}