using Rediska.Protocol.Responses.Visitors;

namespace Rediska.Protocol.Responses
{
    public sealed class SimpleString : DataType
    {
        public SimpleString(string content)
        {
            Content = content;
        }

        public string Content { get; }
        public override string ToString() => $"+{Content}";

        public override T Accept<T>(Visitor<T> visitor) => visitor.Visit(this);
    }
}