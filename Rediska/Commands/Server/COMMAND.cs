namespace Rediska.Commands.Server
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed partial class COMMAND : Command<IReadOnlyList<CommandDescription>>
    {
        private static readonly PlainBulkString name = new PlainBulkString("COMMAND");
        private static readonly BulkString[] request = {name};

        private static readonly ListVisitor<CommandDescription> responseStructure = new ListVisitor<CommandDescription>(
            ArrayExpectation.Singleton,
            CommandDescriptionVisitor.Singleton
        );

        public static COMMAND Singleton { get; } = new COMMAND();
        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => request;
        public override Visitor<IReadOnlyList<CommandDescription>> ResponseStructure => responseStructure;
    }
}