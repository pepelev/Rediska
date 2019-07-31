namespace Rediska.Commands.Sets
{
    using Protocol;
    using Protocol.Visitors;

    public sealed class SCARD : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("SCARD");
        private readonly Key key;

        public SCARD(Key key)
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