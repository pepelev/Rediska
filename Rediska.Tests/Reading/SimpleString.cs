using System.IO;

namespace Rediska.Tests.Reading
{
    public sealed class SimpleString : State
    {
        public static readonly SimpleString Singleton = new SimpleString();

        public override State Transit(Stream stream)
        {
            while (true)
            {
                var @byte = stream.ReadByte();
                if (@byte == -1)
                    return this;

                if (@byte == '\r')
                {
                    return Seek.Get(1);
                }
            }
        }

        public override bool IsTerminal => false;
    }
}