namespace Rediska.Commands.Keys
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class EXPIREAT : Command<ExpireResponse>
    {
        private static readonly PlainBulkString name = new PlainBulkString("EXPIREAT");
        private readonly Key key;
        private readonly UnixTimestamp timestamp;

        public EXPIREAT(Key key, UnixTimestamp timestamp)
        {
            this.key = key;
            this.timestamp = timestamp;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory),
            factory.Create(timestamp.Seconds)
        };

        public override Visitor<ExpireResponse> ResponseStructure => CompositeVisitors.ExpireResult;
    }
}