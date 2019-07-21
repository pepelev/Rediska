using System.IO;

namespace Rediska.Protocol.Responses
{
    public sealed class PlainBulkString : BulkString
    {
        private readonly byte[] content;

        public PlainBulkString(byte[] content)
        {
            this.content = content;
        }

        public override bool IsNull => false;
        public override long Length => content.Length;
        public override void Write(Stream stream) => stream.Write(content, 0, content.Length);
    }
}