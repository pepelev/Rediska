namespace Rediska.Commands.Strings
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class INCR : Command<INCR.Response>
    {
        private static readonly PlainBulkString name = new PlainBulkString("INCR");

        private static readonly Visitor<Response> responseStructure = IntegerExpectation.Singleton
            .Then(valueAfterIncrement => new Response(valueAfterIncrement));

        private readonly Key key;

        public INCR(Key key)
        {
            this.key = key;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory)
        };

        public override Visitor<Response> ResponseStructure => responseStructure;

        public readonly struct Response
        {
            public Response(long valueAfterIncrement)
            {
                ValueAfterIncrement = valueAfterIncrement;
            }

            public long ValueBeforeIncrement => ValueAfterIncrement - 1;
            public long ValueAfterIncrement { get; }
        }
    }
}