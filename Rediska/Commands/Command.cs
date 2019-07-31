using Rediska.Protocol;
using Rediska.Protocol.Visitors;

namespace Rediska.Commands
{
    public abstract class Command<T>
    {
        public abstract DataType Request { get; }
        public abstract Visitor<T> ResponseStructure { get; }
        public override string ToString() => Request.Accept(CommandPrintVisitor.Singleton);
    }
}