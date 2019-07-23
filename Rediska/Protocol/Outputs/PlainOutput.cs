namespace Rediska.Protocol.Outputs
{
    public sealed class PlainOutput : Output
    {
        private readonly byte[] content;
        public int Position { get; private set; }

        public PlainOutput(byte[] content)
        {
            this.content = content;
        }

        public override void Write(Magic magic)
        {
            content[Position++] = magic.ToByte();
        }

        public override void Write(byte[] array)
        {
            System.Array.Copy(array, 0, content, Position, array.Length);
            Position += array.Length;
        }

        public override void Write(long integer)
        {
            if (integer == 0)
            {
                content[Position++] = (byte)'0';
                return;
            }

            if (integer < 0)
            {
                content[Position++] = (byte) '-';
                integer = -integer; // overflow
            }

            const long bound = 1_000_000_000_000_000_000;

            long denominator = 1;

            while (true)
            {
                if (denominator == bound || denominator == integer)
                    break;

                if (integer < denominator)
                {
                    denominator /= 10;
                    break;
                }

                denominator *= 10;
            }

            while (denominator > 0)
            {
                content[Position++] = (byte)(integer / denominator + '0');
                integer %= denominator;
                denominator /= 10;
            }
        }

        public override void WriteCRLF()
        {
            content[Position++] = (byte) '\r';
            content[Position++] = (byte) '\n';
        }
    }
}