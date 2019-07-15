using System.IO;

namespace Rediska.Tests.Reading
{
    public sealed class BulkStringStart : State
    {
        public static readonly BulkStringStart Singleton = new BulkStringStart();

        public override State Transit(Stream stream)
        {
            var @byte = stream.ReadByte();
            if (@byte == -1)
                return this;

            if (@byte == '-')
            {
                //   |
                // "-1\r\n"
                return Seek.Get(3);
            }

            if (@byte == '0')
            {
                //  |
                // 0\r\n\r\n
                return Seek.Get(4);
            }

            return new BulkString(@byte - '0');
        }

        public override bool IsTerminal => false;
    }
}