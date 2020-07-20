using System.IO;
using System.Text;
using Rediska.Protocol.Outputs;
using Rediska.Protocol.Visitors;

namespace Rediska.Protocol
{
    public abstract class BulkString : DataType
    {
        public static BulkString Null { get; } = new NullBulkString();

        public abstract bool IsNull { get; }
        public abstract long Length { get; }
        public abstract void WriteContent(Stream stream);

        public override string ToString() => Encoding.UTF8.GetString(
            this.ToBytes()
        );

        public override T Accept<T>(Visitor<T> visitor) => visitor.Visit(this);
        public static implicit operator BulkString(string value) => new PlainBulkString(value);
        public static implicit operator BulkString(byte[] value) => new PlainBulkString(value);

        private sealed class NullBulkString : BulkString
        {
            public override bool IsNull => true;
            public override long Length => -1;

            public override void WriteContent(Stream stream)
            {
            }

            public override void Write(Output output)
            {
                output.Write(Magic.BulkString);
                output.Write(-1);
                output.WriteCRLF();
            }

            public override string ToString() => nameof(NullBulkString);
        }
    }
}