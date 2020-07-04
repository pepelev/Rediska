namespace Rediska.Commands.Keys
{
    using System.Collections.Generic;
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

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(),
            factory.Create(timestamp.Milliseconds)
        };

        public override Visitor<ExpireResponse> ResponseStructure => CompositeVisitors.ExpireResult;
    }
}