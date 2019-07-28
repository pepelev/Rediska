using Rediska.Protocol;
using Rediska.Protocol.Visitors;

namespace Rediska.Commands
{
    public sealed class ECHO : Command<string>
    {
        private static readonly PlainBulkString name = new PlainBulkString("ECHO");

        private readonly string message;

        public ECHO(string message)
        {
            this.message = message;
        }

        public override DataType Request => new PlainArray(
            name,
            new PlainBulkString(message)
        );

        // todo
        public override Visitor<string> ResponseStructure => new ConstVisitor<string>("foo");
    }
}