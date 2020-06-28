namespace Rediska.Commands.Strings
{
    using Protocol;
    using Protocol.Visitors;

    public sealed class BITCOUNT : Command<BITCOUNT.Response>
    {
        private static readonly PlainBulkString name = new PlainBulkString("BITCOUNT");

        private static readonly Visitor<Response> responseStructure = IntegerExpectation.Singleton
            .Then(numberOfBitsSetToOne => new Response(numberOfBitsSetToOne));

        private readonly Key key;
        private readonly Range range;

        public BITCOUNT(Key key)
            : this(key, Range.Whole)
        {
        }

        public BITCOUNT(Key key, Range range)
        {
            this.key = key;
            this.range = range;
        }

        public override DataType Request => range == Range.Whole
            ? new PlainArray(
                name,
                key.ToBulkString()
            )
            : new PlainArray(
                name,
                key.ToBulkString(),
                range.StartInclusive.ToBulkString(),
                range.EndInclusive.ToBulkString()
            );

        public override Visitor<Response> ResponseStructure => responseStructure;

        public readonly struct Response
        {
            public Response(long numberOfBitsSetToOne)
            {
                NumberOfBitsSetToOne = numberOfBitsSetToOne;
            }

            public long NumberOfBitsSetToOne { get; }
        }
    }
}