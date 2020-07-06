namespace Rediska.Commands.Hashes
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class HVALS : Command<IReadOnlyList<BulkString>>
    {
        private static readonly PlainBulkString name = new PlainBulkString("HVALS");
        private readonly Key key;

        public HVALS(Key key)
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