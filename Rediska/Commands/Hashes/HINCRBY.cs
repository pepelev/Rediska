namespace Rediska.Commands.Hashes
{
    using Protocol;
    using Protocol.Visitors;

    public sealed class HINCRBY : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("HINCRBY");
        private readonly Key key;
        private readonly Key field;
        private readonly long increment;

        public HINCRBY(Key key, Key field, long increment)
        {
            this.key = key;
            this.field = field;
            this.increment = increment;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString(),
            field.ToBulkString(),
            increment.ToBulkString()
        );

        public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;
    }
}