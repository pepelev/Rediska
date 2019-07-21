using System.Globalization;
using System.IO;
using System.Text;

namespace Rediska.Protocol
{
    public sealed class StreamOutput : Output
    {
        private readonly Stream stream;

        public StreamOutput(Stream stream)
        {
            this.stream = stream;
        }

        public override void Write(Magic magic) => stream.WriteByte(magic.ToByte());
        public override void Write(byte[] array) => stream.Write(array, 0, array.Length);

        public override void Write(long integer)
        {
            var @string = integer.ToString(CultureInfo.InvariantCulture);
            var bytes = Encoding.UTF8.GetBytes(@string);
            stream.Write(bytes, 0, bytes.Length);
        }

        public override void WriteCRLF()
        {
            stream.WriteByte((byte)'\r');
            stream.WriteByte((byte)'\n');
        }
    }
}