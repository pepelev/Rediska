using Rediska.Protocol;
using Rediska.Protocol.Visitors;

namespace Rediska.Commands.Transactions
{
    public sealed class EXEC : Command<Array>
    {
        public static EXEC Singleton { get; } = new EXEC();

        private static readonly PlainArray request = new PlainArray(
            new PlainBulkString("EXEC")
        );

        public override DataType Request => request;
        public override Visitor<Array> ResponseStructure => ArrayExpectation2.Singleton;
    }
}