using Rediska.Protocol;
using Rediska.Protocol.Visitors;

namespace Rediska.Commands.Sets
{
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

        public override DataType Request => new PlainArray(
            name,
            source.ToBulkString(),
            destination.ToBulkString(),
            member
        );

        public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;
    }
}