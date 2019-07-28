using Rediska.Protocol;
using Rediska.Protocol.Visitors;

namespace Rediska.Commands.Sets
{
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

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString(),
            member
        );

        public override Visitor<bool> ResponseStructure => IntegerExpectation.Singleton
            .Then(response => response == 1);
    }
}