namespace Rediska.Protocol
{
    using System.IO;
    using System.Text;
    using Outputs;

    public sealed class PlainBulkString : BulkString
    {
        private readonly byte[] content;

        public PlainBulkString(string content)
            : this(Encoding.UTF8.GetBytes(content))
        {
        }

        public PlainBulkString(byte[] content)
        {
            this.content = content;
        }

        public override bool IsNull => false;
        public override long Length => content.Length;
        public static PlainBulkString Empty { get; } = new PlainBulkString(new byte[0]);
        public override void WriteContent(Stream stream) => stream.Write(content, 0, content.Length);

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