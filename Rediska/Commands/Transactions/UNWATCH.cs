using Rediska.Protocol;
using Rediska.Protocol.Visitors;
using Rediska.Utils;

namespace Rediska.Commands.Transactions
{
    public sealed class UNWATCH : Command<None>
    {
        private static readonly PlainArray name = new PlainArray(
            new PlainBulkString("UNWATCH")
        );

        public static UNWATCH Singleton { get; } = new UNWATCH();

        public override DataType Request => name;
        public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
    }
}