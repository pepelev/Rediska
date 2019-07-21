using System.IO;
using System.Text;
using Rediska.Protocol.Responses.Visitors;

namespace Rediska.Protocol.Responses
{
    public abstract class BulkString : DataType
    {
        public static BulkString Null { get; } = new NullBulkString();

        public abstract bool IsNull { get; }
        public abstract long Length { get; }
        public abstract void Write(Stream stream);

        public override string ToString() => Encoding.UTF8.GetString(
            this.ToBytes()
        );

        public override T Accept<T>(Visitor<T> visitor) => visitor.Visit(this);

        private sealed class NullBulkString : BulkString
        {
            public override bool IsNull => true;
            public override long Length => -1;

            public override void Write(Stream stream)
            {
            }

            public override string ToString() => nameof(NullBulkString);
        }
    }
}