namespace Rediska.Commands.Keys
{
    using Protocol;
    using Protocol.Visitors;

    public sealed class PEXPIREAT : Command<ExpireResponse>
    {
        private static readonly PlainBulkString name = new PlainBulkString("PEXPIREAT");
        private readonly Key key;
        private readonly UnixMillisecondsTimestamp timestamp;

        public PEXPIREAT(Key key, UnixMillisecondsTimestamp timestamp)
        {
            this.key = key;
            this.timestamp = timestamp;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString(),
            timestamp.ToBulkString()
        );

        public override Visitor<ExpireResponse> ResponseStructure => CompositeVisitors.ExpireResult;
    }
}