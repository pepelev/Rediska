using Rediska.Protocol;
using Rediska.Protocol.Visitors;
using Rediska.Utils;

namespace Rediska.Commands.Transactions
{
    public sealed class MULTI : Command<None>
    {
        private static readonly PlainArray request = new PlainArray(
            new PlainBulkString("MULTI")
        );

        public static MULTI Singleton { get; } = new MULTI();

        public override DataType Request => request;
        public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
    }
}