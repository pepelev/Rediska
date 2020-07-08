namespace Rediska.Commands.SortedSets
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class ZPOPMIN : Command<IReadOnlyList<(BulkString Member, double Score)>>
    {
        private static readonly PlainBulkString name = new PlainBulkString("ZPOPMIN");
        private readonly Key key;
        private readonly long count;

        public ZPOPMIN(Key key, long count = 1)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Must be positive");

            this.key = key;
            this.count = count;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) =>
            ZPOP.Request(name, key, count, factory);

        public override Visitor<IReadOnlyList<(BulkString Member, double Score)>> ResponseStructure =>
            CompositeVisitors.SortedSetEntryList;
    }
}