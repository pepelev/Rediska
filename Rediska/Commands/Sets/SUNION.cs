namespace Rediska.Commands.Sets
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class SUNION : Command<IReadOnlyList<BulkString>>
    {
        private static readonly PlainBulkString name = new PlainBulkString("SUNION");
        private readonly IReadOnlyList<Key> keys;

        public SUNION(IReadOnlyList<Key> keys)
        {
            this.keys = keys;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory)
        {
            return new PrefixedList<BulkString>(
                name,
                new KeyList(factory, keys)
            );
        }

        public override Visitor<IReadOnlyList<BulkString>> ResponseStructure => CompositeVisitors.BulkStringList;
    }
}