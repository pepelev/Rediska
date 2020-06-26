namespace Rediska.Commands.Hashes
{
    using Protocol;
    using Protocol.Visitors;

    public sealed class HGET : Command<BulkString>
    {
        private static readonly PlainBulkString name = new PlainBulkString("HGET");
        private readonly Key key;
        private readonly Key field;

        public HGET(Key key, Key field)
        {
            this.key = key;
            this.field = field;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString(),
            field.ToBulkString()
        );

        public override Visitor<BulkString> ResponseStructure => BulkStringExpectation.Singleton;
    }
}