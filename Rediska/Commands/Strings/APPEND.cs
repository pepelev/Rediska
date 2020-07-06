namespace Rediska.Commands.Strings
{
    using System.Collections.Generic;
    using System.Globalization;
    using Protocol;
    using Protocol.Visitors;

    public sealed class APPEND : Command<APPEND.Response>
    {
        private static readonly PlainBulkString name = new PlainBulkString("APPEND");

        private static readonly Visitor<Response> responseStructure = IntegerExpectation.Singleton
            .Then(stringLengthAfterAppend => new Response(stringLengthAfterAppend));

        private readonly Key key;
        private readonly BulkString value;

        public APPEND(Key key, BulkString value)
        {
            this.key = key;
            this.value = value;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory),
            value
        };

        public override Visitor<Response> ResponseStructure => responseStructure;

        public readonly struct Response
        {
            public Response(long response)
            {
                StringLengthAfterAppend = response;
            }

            public long StringLengthAfterAppend { get; }
            public override string ToString() => StringLengthAfterAppend.ToString(CultureInfo.InvariantCulture);
        }
    }
}