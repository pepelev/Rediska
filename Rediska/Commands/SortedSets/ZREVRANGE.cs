namespace Rediska.Commands.SortedSets
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed partial class ZREVRANGE : Command<IReadOnlyList<BulkString>>
    {
        private static readonly PlainBulkString name = new PlainBulkString("ZREVRANGE");
        private readonly Key key;
        private readonly Range range;

        public ZREVRANGE(Key key, Range range)
        {
            this.key = key;
            this.range = range;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory),
            range.StartInclusive.ToBulkString(factory),
            range.EndInclusive.ToBulkString(factory)
        };

        public override Visitor<IReadOnlyList<BulkString>> ResponseStructure => CompositeVisitors.BulkStringList;
    }
}