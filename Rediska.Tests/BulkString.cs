using System.Text;
using Rediska.Tests.Checks;

namespace Rediska.Tests
{
    public sealed class BulkString : DataType
    {
        private readonly byte[] content;

        public BulkString(string content)
            : this(Encoding.ASCII.GetBytes(content))
        {
        }

        public BulkString(byte[] content)
        {
            this.content = content;
        }

        public override void Write(Output output)
        {
            output.Write(Magic.BulkString);
            output.Write(content.Length);
            output.WriteCRLF();
            output.Write(content);
            output.WriteCRLF();
        }
    }
}