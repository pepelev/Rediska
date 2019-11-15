namespace Rediska.Commands.Lists
{
    using Protocol;
    using Protocol.Visitors;

    public sealed class LLEN : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("LLEN");
        private readonly Key key;

        public LLEN(Key key)
        {
            this.key = key;
        }

        public override DataType Request => new PlainArray(name, key.ToBulkString());
        public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;
    }
}