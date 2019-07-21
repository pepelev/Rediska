using Rediska.Protocol.Requests;
using Rediska.Protocol.Responses.Visitors;

namespace Rediska.Commands
{
    public abstract class Command<T>
    {
        public abstract DataType Request { get; }
        public abstract Visitor<T> ResponseStructure { get; }
        public override string ToString() => Request.ToString();
    }
}