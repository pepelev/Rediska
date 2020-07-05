namespace Rediska.Commands.Sets
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class SMEMBERS : Command<IReadOnlyList<BulkString>>
    {
        private static readonly PlainBulkString name = new PlainBulkString("SMEMBERS");
        private readonly Key key;

        public SMEMBERS(Key key)
        {
            this.key = key;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory)
        };

        public override Visitor<IReadOnlyList<BulkString>> ResponseStructure => CompositeVisitors.BulkStringList;
    }
}