namespace Rediska.Commands.Strings
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class DECRBY : Command<DECRBY.Response>
    {
        private static readonly PlainBulkString name = new PlainBulkString("DECRBY");
        private readonly Key key;
        private readonly long decrement;

        public DECRBY(Key key, long decrement)
        {
            this.key = key;
            this.decrement = decrement;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory),
            factory.Create(decrement)
        };

        public override Visitor<Response> ResponseStructure => IntegerExpectation.Singleton
            .Then(valueAfterDecrement => new Response(decrement, valueAfterDecrement));

        public readonly struct Response
        {
            public Response(long decrement, long valueAfterDecrement)
            {
                Decrement = decrement;
                ValueAfterDecrement = valueAfterDecrement;
            }

            public long Decrement { get; }
            public long ValueBeforeDecrement => ValueAfterDecrement - Decrement;
            public long ValueAfterDecrement { get; }
        }
    }
}