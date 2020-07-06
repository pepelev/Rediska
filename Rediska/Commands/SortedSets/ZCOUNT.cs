namespace Rediska.Commands.SortedSets
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class ZCOUNT : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("ZCOUNT");
        private readonly Key key;
        private readonly ScoreInterval interval;

        public ZCOUNT(Key key, ScoreInterval interval)
        {
            this.key = key;
            this.interval = interval;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory),
            interval.Min.ToBulkString(factory),
            interval.Max.ToBulkString(factory)
        };

        public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;
    }
}