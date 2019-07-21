using Rediska.Tests.Protocol.Requests;
using Rediska.Tests.Protocol.Responses.Visitors;

namespace Rediska.Tests
{
    public abstract class Command<T>
    {
        public abstract DataType Request { get; }
        public abstract Visitor<T> ResponseStructure { get; }
        public override string ToString() => Request.ToString();
    }
}