namespace Rediska.Commands.Lists
{
    using Protocol;
    using Protocol.Visitors;

    public sealed class LINDEX : Command<BulkString>
    {
        private static readonly PlainBulkString name = new PlainBulkString("LINDEX");
        private readonly Key key;
        private readonly Index index;

        public LINDEX(Key key, Index index)
        {
            this.key = key;
            this.index = index;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString(),
            index.ToBulkString()
        );

        public override Visitor<BulkString> ResponseStructure => BulkStringExpectation.Singleton;
    }
}