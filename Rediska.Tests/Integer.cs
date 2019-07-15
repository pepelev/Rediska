using Rediska.Tests.Checks;

namespace Rediska.Tests
{
    public sealed class Integer : DataType
    {
        private readonly long value;

        public Integer(long value)
        {
            this.value = value;
        }

        public override void Write(Output output)
        {
            output.Write(Magic.Integer);
            output.Write(value);
            output.WriteCRLF();
        }

        public override string ToString() => value.ToString();
    }
}