namespace Rediska.Commands.Strings
{
    using Protocol;
    using Protocol.Visitors;

    public sealed class STRLEN : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("STRLEN");
        private readonly Key key;

        public STRLEN(Key key)
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