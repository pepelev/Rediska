namespace Rediska.Commands.Streams
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class XRANGE : Command<IReadOnlyList<Entry>>
    {
        private static readonly PlainBulkString name = new PlainBulkString("XRANGE");
        private readonly Key key;
        private readonly Interval interval;
        private readonly Count count;

        public XRANGE(Key key, Interval interval)
            : this(key, interval, Count.Unbound)
        {
        }

        public XRANGE(Key key, Interval interval, Count count)
        {
            this.key = key;
            this.interval = interval;
            this.count = count;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory)
        {
            yield return name;
            yield return key.ToBulkString(factory);
            if (interval.StartInclusive == Id.Minimum)
                yield return RangeConstants.Minimum;
            else
                yield return interval.StartInclusive.ToBulkString(factory, Id.Print.SkipMinimalLow);

            if (interval.EndInclusive == Id.Maximum)
                yield return RangeConstants.Maximum;
            else
                yield return interval.EndInclusive.ToBulkString(factory, Id.Print.SkipMaximalLow);

            foreach (var argument in count.Arguments(factory))
            {
                yield return argument;
            }
        }

        public override Visitor<IReadOnlyList<Entry>> ResponseStructure => RangeConstants.ResponseStructure;
    }
}