namespace Rediska.Commands.Utility
{
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class IgnoringResponse<T> : Command<None>
    {
        private static readonly ConstVisitor<None> responseStructure = new ConstVisitor<None>(new None());

        private readonly Command<T> command;

        public IgnoringResponse(Command<T> command)
        {
            this.command = command;
        }

        public override DataType Request => command.Request;
        public override Visitor<None> ResponseStructure => responseStructure;
    }
}