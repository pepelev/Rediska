using Rediska.Protocol.Responses.Visitors;

namespace Rediska.Protocol.Responses
{
    public abstract class DataType
    {
        public abstract T Accept<T>(Visitor<T> visitor);
    }
}