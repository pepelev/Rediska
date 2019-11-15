namespace Rediska.Commands.Lists
{
    using Protocol;
    using Protocol.Visitors;

    public sealed class RPOPLPUSH : Command<BulkString>
    {
        private static readonly PlainBulkString name = new PlainBulkString("RPOPLPUSH");
        private readonly Key source;
        private readonly Key destination;

        public RPOPLPUSH(Key source, Key destination)
        {
            this.source = source;
            this.destination = destination;
        }

        public override DataType Request => new PlainArray(
            name,
            source.ToBulkString(),
            destination.ToBulkString()
        );

        public override Visitor<BulkString> ResponseStructure => BulkStringExpectation.Singleton;
        public static RPOPLPUSH Rotate(Key key) => new RPOPLPUSH(key, key);
    }
}