using Rediska.Protocol.Requests;
using Rediska.Protocol.Responses.Visitors;

namespace Rediska.Commands.Sets
{
    public sealed class SCARD : Command<long>
    {
        private static readonly BulkString name = new BulkString("SCARD");

        private readonly Key key;

        public SCARD(Key key)
        {
            this.key = key;
        }

        public override DataType Request => new Array(
            name,
            key.ToBulkString()
        );

        public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;
    }
}