namespace Rediska.Commands.Hashes
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class HGETALL : Command<IReadOnlyList<(BulkString Field, BulkString Value)>>
    {
        private static readonly BulkString name = new PlainBulkString("HGETALL");
        private readonly Key key;

        public HGETALL(Key key)
        {
            this.key = key;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory)
        };

        public override Visitor<IReadOnlyList<(BulkString Field, BulkString Value)>> ResponseStructure =>
            CompositeVisitors.HashEntryList;
    }
}