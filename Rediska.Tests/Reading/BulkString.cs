using System.IO;
using static Rediska.Tests.Reading.Sizes;

namespace Rediska.Tests.Reading
{
    public sealed class BulkString : State
    {
        private readonly long partiallyParsedLength;

        public BulkString(long partiallyParsedLength)
        {
            this.partiallyParsedLength = partiallyParsedLength;
        }

        public override State Transit(Stream stream)
        {
            var length = partiallyParsedLength;
            while (true)
            {
                var @byte = stream.ReadByte();
                if (@byte == -1)
                {
                    return length == partiallyParsedLength
                        ? this
                        : new BulkString(length);
                }

                if (@byte == '\r')
                {
                    // \n content \r\n
                    return Seek.Get(length + LF + CRLF);
                }

                length = length * 10 + @byte - '0';
            }
        }

        public override bool IsTerminal => false;
    }
}