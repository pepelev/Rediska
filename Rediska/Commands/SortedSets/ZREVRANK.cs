namespace Rediska.Commands.SortedSets
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class ZREVRANK : Command<long?>
    {
        private static readonly PlainBulkString name = new PlainBulkString("ZREVRANK");
        private readonly Key key;
        private readonly BulkString member;

        public ZREVRANK(Key key, BulkString member)
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

        public override Visitor<long?> ResponseStructure => NullableIntegerExpectation.Singleton;
    }
}