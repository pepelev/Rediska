namespace Rediska.Commands.Keys
{
    using Protocol;
    using Protocol.Visitors;

    public sealed class DUMP : Command<BulkString>
    {
        private static readonly PlainBulkString name = new PlainBulkString("DUMP");
        private readonly Key key;

        public DUMP(Key key)
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