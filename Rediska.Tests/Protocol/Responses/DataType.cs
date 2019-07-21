using Rediska.Tests.Protocol.Responses.Visitors;

namespace Rediska.Tests.Protocol.Responses
{
    public abstract class DataType
    {
        public abstract T Accept<T>(Visitor<T> visitor);
    }
}