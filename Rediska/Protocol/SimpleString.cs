using System.Text;
using Rediska.Protocol.Outputs;
using Rediska.Protocol.Visitors;

namespace Rediska.Protocol
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

        public override void Write(Output output)
        {
            output.Write(Magic.SimpleString);
            var bytes = Encoding.ASCII.GetBytes(Content);
            output.Write(bytes);
            output.WriteCRLF();
        }
    }
}