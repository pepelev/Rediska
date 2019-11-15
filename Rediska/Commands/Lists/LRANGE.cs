namespace Rediska.Commands.Lists
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class LRANGE : Command<IReadOnlyList<BulkString>>
    {
        private static readonly PlainBulkString name = new PlainBulkString("LRANGE");
        private readonly Key key;
        private readonly Index start;
        private readonly Index stop;

        public LRANGE(Key key, Index start, Index stop)
        {
            this.key = key;
            this.start = start;
            this.stop = stop;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString(),
            start.ToBulkString(),
            stop.ToBulkString()
        );

        public override Visitor<IReadOnlyList<BulkString>> ResponseStructure => CompositeVisitors.BulkStringList;
    }
}