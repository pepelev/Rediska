using System.IO;
using System.Text;

namespace Rediska.Tests
{
    public abstract partial class Key
    {
        public sealed class BulkString : Key
        {
            private readonly Protocol.Responses.BulkString content;

            public BulkString(Protocol.Responses.BulkString content)
            {
                this.content = content;
            }

            public override byte[] ToBytes()
            {
                using (var stream = new MemoryStream())
                {
                    content.Write(stream);
                    return stream.ToArray();
                }
            }

            public override string ToString() => Encoding.UTF8.GetString(ToBytes());
        }
    }
}