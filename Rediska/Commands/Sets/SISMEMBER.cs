namespace Rediska.Commands.Sets
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class SISMEMBER : Command<bool>
    {
        private static readonly PlainBulkString name = new PlainBulkString("SISMEMBER");
        private readonly Key key;
        private readonly BulkString member;

        public SISMEMBER(Key key, BulkString member)
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

        public override Visitor<bool> ResponseStructure => IntegerExpectation.Singleton
            .Then(response => response == 1);
    }
}