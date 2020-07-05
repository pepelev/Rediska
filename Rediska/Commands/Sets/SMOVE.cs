namespace Rediska.Commands.Sets
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class SMOVE : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("SMOVE");
        private readonly Key source;
        private readonly Key destination;
        private readonly BulkString member;

        public SMOVE(Key source, Key destination, BulkString member)
        {
            this.source = source;
            this.destination = destination;
            this.member = member;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            source.ToBulkString(factory),
            destination.ToBulkString(factory),
            member
        };

        public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;
    }
}