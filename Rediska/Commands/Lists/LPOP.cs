namespace Rediska.Commands.Lists
{
    using Protocol;
    using Protocol.Visitors;

    public sealed class LPOP : Command<BulkString>
    {
        private static readonly PlainBulkString name = new PlainBulkString("LPOP");
        private readonly Key key;

        public LPOP(Key key)
        {
            this.key = key;
        }

        public override DataType Request => new PlainArray(name, key.ToBulkString());
        public override Visitor<BulkString> ResponseStructure => BulkStringExpectation.Singleton;
    }
}