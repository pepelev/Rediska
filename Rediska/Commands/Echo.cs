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

        public override Visitor<string> ResponseStructure =>
            BulkStringExpectation.Singleton.Then(bulk => bulk.ToString());
    }
}