namespace Rediska.Commands.Lists
{
    using Protocol;
    using Protocol.Visitors;

    public sealed class BRPOPLPUSH : Command<BulkString>
    {
        private static readonly PlainBulkString name = new PlainBulkString("BRPOPLPUSH");
        private readonly Key source;
        private readonly Key destination;
        private readonly Timeout timeout;

        public BRPOPLPUSH(Key source, Key destination, Timeout timeout)
        {
            this.source = source;
            this.destination = destination;
            this.timeout = timeout;
        }

        public override DataType Request => new PlainArray(
            name,
            source.ToBulkString(),
            destination.ToBulkString(),
            timeout.ToBulkString()
        );

        public override Visitor<BulkString> ResponseStructure => BulkStringExpectation.Singleton;
    }
}