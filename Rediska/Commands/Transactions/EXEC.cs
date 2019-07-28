using Rediska.Protocol;
using Rediska.Protocol.Visitors;

namespace Rediska.Commands.Transactions
{
    public sealed class EXEC : Command<Array>
    {
        private static readonly PlainArray request = new PlainArray(
            new PlainBulkString("EXEC")
        );

        public override DataType Request => request;
        public override Visitor<Array> ResponseStructure => ArrayExpectation2.Singleton;
    }
}