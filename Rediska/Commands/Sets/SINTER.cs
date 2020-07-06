namespace Rediska.Commands.Sets
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class SINTER : Command<IReadOnlyList<BulkString>>
    {
        private static readonly PlainBulkString name = new PlainBulkString("SINTER");
        private readonly IReadOnlyList<Key> keys;

        public SINTER(IReadOnlyList<Key> keys)
        {
            this.keys = keys;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new PrefixedList<BulkString>(
            name,
            new KeyList(factory, keys)
        );

        public override Visitor<IReadOnlyList<BulkString>> ResponseStructure => CompositeVisitors.BulkStringList;
    }
}