namespace Rediska.Commands
{
    public sealed class ECHO : Command<string>
    {
        private readonly string message;

        public ECHO(string message)
        {
            this.message = message;
        }

        public override DataType Request => new Array(
            new BulkString("ECHO"),
            new BulkString(message)
        );

        public override Visitor<string> ResponseStructure => new ConstVisitor<string>("foo");
    }
}