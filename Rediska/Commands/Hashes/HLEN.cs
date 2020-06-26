namespace Rediska.Commands.Hashes
{
    using Protocol;
    using Protocol.Visitors;

    public sealed class HLEN : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("HLEN");
        private readonly Key key;

        public HLEN(Key key)
        {
            this.key = key;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString()
        );

        public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;
    }
}