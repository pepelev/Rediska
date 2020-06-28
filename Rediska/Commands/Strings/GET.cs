namespace Rediska.Commands.Strings
{
    using Protocol;
    using Protocol.Visitors;

    public sealed class GET : Command<BulkString>
    {
        private static readonly PlainBulkString name = new PlainBulkString("GET");
        private readonly Key key;

        public GET(Key key)
        {
            this.key = key;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString()
        );

        public override Visitor<BulkString> ResponseStructure => BulkStringExpectation.Singleton;
    }
}