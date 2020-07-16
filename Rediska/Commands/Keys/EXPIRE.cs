namespace Rediska.Commands.Keys
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class EXPIRE : Command<ExpireResponse>
    {
        private static readonly PlainBulkString name = new PlainBulkString("EXPIRE");
        private readonly Key key;
        private readonly long seconds;

        // todo seconds to semantic struct
        public EXPIRE(Key key, long seconds)
        {
            this.key = key;
            this.seconds = seconds;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory),
            factory.Create(seconds)
        };

        public override Visitor<ExpireResponse> ResponseStructure => CompositeVisitors.ExpireResult;
    }
}