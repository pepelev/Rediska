using System.Linq;

namespace Rediska.Protocol.Visitors
{
    public sealed class CommandPrintVisitor : Visitor<string>
    {
        public static CommandPrintVisitor Singleton { get; } = new CommandPrintVisitor();

        public override string Visit(Integer integer) => integer.Value.ToString();
        public override string Visit(SimpleString simpleString) => simpleString.Content;
        public override string Visit(Error error) => error.Content;

        public override string Visit(Array array) => string.Join(
            " ",
            array.Select(item => item.Accept(Singleton))
        );

        public override string Visit(BulkString bulkString) => bulkString.ToString();
    }
}