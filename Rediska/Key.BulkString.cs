namespace Rediska
{
    using System.IO;
    using System.Text;
    using Commands;

    public abstract partial class Key
    {
        public sealed class BulkString : Key
        {
            private readonly Protocol.BulkString content;

            public BulkString(Protocol.BulkString content)
            {
                this.content = content;
            }

            public override byte[] ToBytes()
            {
                using var stream = new MemoryStream();
                content.WriteContent(stream);
                return stream.ToArray();
            }

            public override Protocol.BulkString ToBulkString(BulkStringFactory factory) => content;
            public override string ToString() => Encoding.UTF8.GetString(ToBytes());
        }
    }
}