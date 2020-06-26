namespace Rediska.Commands.Keys
{
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class RENAME : Command<None>
    {
        private static readonly PlainBulkString name = new PlainBulkString("RENAME");
        private readonly Key key;
        private readonly Key newKey;

        public RENAME(Key key, Key newKey)
        {
            this.key = key;
            this.newKey = newKey;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString(),
            newKey.ToBulkString()
        );

        // todo parse error
        public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
    }
}