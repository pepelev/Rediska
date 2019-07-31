using Rediska.Protocol;
using Rediska.Protocol.Visitors;
using Rediska.Utils;

namespace Rediska.Commands.Transactions
{
    public sealed class DISCARD : Command<None>
    {
        private static readonly PlainArray name = new PlainArray(
            new PlainBulkString("DISCARD")
        );

        public static DISCARD Singleton { get; } = new DISCARD();

        public override DataType Request => name;
        public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
    }
}