namespace Rediska.Commands.Lists
{
    using Protocol;
    using Protocol.Visitors;

    public sealed class RPOP : Command<BulkString>
    {
        private static readonly PlainBulkString name = new PlainBulkString("RPOP");
        private readonly Key key;

        public RPOP(Key key)
        {
            this.key = key;
        }

        public override DataType Request => new PlainArray(name, key.ToBulkString());
        public override Visitor<BulkString> ResponseStructure => BulkStringExpectation.Singleton;
    }
}