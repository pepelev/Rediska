using System.IO;

namespace Rediska.Tests.Reading
{
    public sealed class ArrayStart : State
    {
        public static ArrayStart Singleton { get; } = new ArrayStart();

        public override State Transit(Stream stream)
        {
            var @byte = stream.ReadByte();
            if (@byte == -1)
                return this;

            if (@byte == '-')
            {
                //  |
                // -1\r\n
                return Seek.Get(3);
            }

            if (@byte == '0')
            {
                //  |
                // 0\r\n
                return Seek.Get(2);
            }

            return new ArrayLength(@byte - '0');
        }

        public override bool IsTerminal => false;
    }
}