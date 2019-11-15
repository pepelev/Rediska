namespace Rediska.Commands.Keys
{
    using Protocol;
    using Protocol.Visitors;

    public sealed class EXPIREAT : Command<ExpireResult>
    {
        private static readonly PlainBulkString name = new PlainBulkString("EXPIREAT");
        private readonly Key key;
        private readonly UnixTimestamp timestamp;

        public EXPIREAT(Key key, UnixTimestamp timestamp)
        {
            this.key = key;
            this.timestamp = timestamp;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString(),
            timestamp.ToBulkString()
        );

        public override Visitor<ExpireResult> ResponseStructure => CompositeVisitors.ExpireResult;
    }
}