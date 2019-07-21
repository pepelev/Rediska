using System.Text;

namespace Rediska.Protocol.Requests
{
    public sealed class BulkString : DataType
    {
        private readonly byte[] content;

        public BulkString(string content)
            : this(Encoding.UTF8.GetBytes(content))
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

        public override string ToString() => Encoding.UTF8.GetString(content);
    }
}