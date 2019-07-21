using Rediska.Tests.Protocol.Responses.Visitors;

namespace Rediska.Tests.Protocol.Responses
{
    public sealed class Integer : DataType
    {
        public Integer(long value)
        {
            Value = value;
        }

        public long Value { get; }
        public override string ToString() => $":{Value}";

        public override T Accept<T>(Visitor<T> visitor) => visitor.Visit(this);
    }
}