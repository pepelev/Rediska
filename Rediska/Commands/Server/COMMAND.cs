namespace Rediska.Commands.Server
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class COMMAND : Command<IReadOnlyList<CommandDescription>>
    {
        private static readonly PlainBulkString name = new PlainBulkString("COMMAND");
        private static readonly PlainArray request = new PlainArray(name);
        public static COMMAND Singleton { get; } = new COMMAND();
        public override DataType Request => request;

        public override Visitor<IReadOnlyList<CommandDescription>> ResponseStructure => new ListVisitor<CommandDescription>(
            ArrayExpectation.Singleton,
            CommandDescriptionVisitor.Singleton
        );
    }
}