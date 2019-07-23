using Rediska.Protocol;
using Rediska.Protocol.Visitors;

namespace Rediska.Commands.Sets
{
    public sealed class SCARD : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("SCARD");

        private readonly Key key;

        public SCARD(Key key)
        {
            this.key = key;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString()
        );

        public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;
    }
}