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

            internal static readonly Visitor<IReadOnlyList<(BulkString Member, double Score)>> responseStructure
                = CompositeVisitors.BulkStringList.Then(
                    list => new ProjectingReadOnlyList<
                        (BulkString Member, BulkString Score),
                        (BulkString Member, double Score)>(
                        new PairsList<BulkString>(
                            list
                        ),
                        pair => (pair.Member, pair.Score.ToDouble())
                    ) as IReadOnlyList<(BulkString Member, double Score)>
                );

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

            public override Visitor<IReadOnlyList<(BulkString Member, double Score)>> ResponseStructure => responseStructure;
        }
    }
}