using Rediska.Protocol.Outputs;
using Rediska.Protocol.Visitors;

namespace Rediska.Protocol
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

        public override void Write(Output output)
        {
            output.Write(Magic.Integer);
            output.Write(Value);
            output.WriteCRLF();
        }
    }
}