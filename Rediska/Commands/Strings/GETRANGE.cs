namespace Rediska.Commands.Strings
{
    using Protocol;
    using Protocol.Visitors;

    public sealed class GETRANGE : Command<BulkString>
    {
        private static readonly PlainBulkString name = new PlainBulkString("GETRANGE");
        private readonly Key key;
        private readonly Range range;

        public GETRANGE(Key key, Range range)
        {
            this.key = key;
            this.range = range;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString(),
            range.StartInclusive.ToBulkString(),
            range.EndInclusive.ToBulkString()
        );

        public override Visitor<BulkString> ResponseStructure => BulkStringExpectation.Singleton;
    }
}