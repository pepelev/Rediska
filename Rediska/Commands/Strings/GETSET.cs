namespace Rediska.Commands.Strings
{
    using Protocol;
    using Protocol.Visitors;

    public sealed class GETSET : Command<BulkString>
    {
        private static readonly PlainBulkString name = new PlainBulkString("GETSET");
        private readonly Key key;
        private readonly BulkString value;

        public GETSET(Key key, BulkString value)
        {
            this.key = key;
            this.value = value;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString(),
            value
        );

        public override Visitor<BulkString> ResponseStructure => BulkStringExpectation.Singleton;
    }
}