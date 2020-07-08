namespace Rediska.Commands.SortedSets
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed partial class ZRANGE
    {
        public sealed class WITHSCORES : Command<IReadOnlyList<(BulkString Member, double Score)>>
        {
            internal static readonly PlainBulkString withscores = new PlainBulkString("WITHSCORES");

            private readonly Key key;
            private readonly Range range;

            public WITHSCORES(Key key, Range range)
            {
                this.key = key;
                this.range = range;
            }

            public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
            {
                name,
                key.ToBulkString(factory),
                range.StartInclusive.ToBulkString(factory),
                range.EndInclusive.ToBulkString(factory),
                withscores
            };

            public override Visitor<IReadOnlyList<(BulkString Member, double Score)>> ResponseStructure =>
                CompositeVisitors.SortedSetEntryList;
        }
    }
}