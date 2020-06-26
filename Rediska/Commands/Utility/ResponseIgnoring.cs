namespace Rediska.Commands.Utility
{
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    internal static class ResponseIgnoring
    {
        public static ConstVisitor<None> Structure { get; } = new ConstVisitor<None>(new None());
    }

    public sealed class ResponseIgnoring<T> : Command<None>
    {
        private readonly Command<T> command;

        public ResponseIgnoring(Command<T> command)
        {
            this.command = command;
        }

        public override DataType Request => command.Request;
        public override Visitor<None> ResponseStructure => ResponseIgnoring.Structure;
    }
}