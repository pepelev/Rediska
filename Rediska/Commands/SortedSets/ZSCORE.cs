namespace Rediska.Commands.SortedSets
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class ZSCORE : Command<double?>
    {
        private static readonly PlainBulkString name = new PlainBulkString("ZSCORE");

        private static readonly Visitor<double?> responseStructure = BulkStringExpectation.Singleton
            .Then(score => score.ToDoubleOrNull());

        private readonly Key key;
        private readonly BulkString member;

        public ZSCORE(Key key, BulkString member)
        {
            this.key = key;
            this.member = member;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory),
            member
        };

        public override Visitor<double?> ResponseStructure => responseStructure;
    }
}