namespace Rediska.Commands.Strings
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class INCRBYFLOAT : Command<INCRBYFLOAT.Response>
    {
        private static readonly PlainBulkString name = new PlainBulkString("INCRBYFLOAT");

        private static readonly Visitor<Response> responseStructure = DoubleExpectation.Singleton
            .Then(valueAfterIncrement => new Response(valueAfterIncrement));

        private readonly Key key;
        private readonly double increment;

        public INCRBYFLOAT(Key key, double increment)
        {
            this.key = key;
            this.increment = increment;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory),
            factory.Create(increment)
        };

        public override Visitor<Response> ResponseStructure => responseStructure;

        public readonly struct Response
        {
            public Response(double valueAfterIncrement)
            {
                ValueAfterIncrement = valueAfterIncrement;
            }

            public double ValueAfterIncrement { get; }
        }
    }
}