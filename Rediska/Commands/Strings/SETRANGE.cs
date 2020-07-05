namespace Rediska.Commands.Strings
{
    using System.Collections.Generic;
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

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory),
            factory.Create(offset),
            value
        };

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