namespace Rediska.Tests
{
    public abstract class Output
    {
        public static Output Null { get; } = new NullOutput();

        public abstract void Write(Magic magic);
        public abstract void Write(byte[] array);
        public abstract void Write(long integer);
        public abstract void WriteCRLF();

        private sealed class NullOutput : Output
        {
            public override void Write(Magic magic)
            {
            }

            public override void Write(byte[] array)
            {
            }

            public override void Write(long integer)
            {
            }

            public override void WriteCRLF()
            {
            }
        }
    }
}