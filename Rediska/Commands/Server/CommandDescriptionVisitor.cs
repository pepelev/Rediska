namespace Rediska.Commands.Server
{
    using Protocol;
    using Protocol.Visitors;

    public sealed class CommandDescriptionVisitor : Expectation<CommandDescription>
    {
        public static CommandDescriptionVisitor Singleton { get; } = new CommandDescriptionVisitor();
        public override string Message => "Array";
        public override CommandDescription Visit(Array array) => new CommandDescription(array);
    }
}