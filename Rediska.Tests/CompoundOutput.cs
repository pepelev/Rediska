using Rediska.Tests.Checks;

namespace Rediska.Tests
{
    public sealed class CompoundOutput : Output
    {
        private readonly Output first;
        private readonly Output second;

        public CompoundOutput(Output first, Output second)
        {
            this.first = first;
            this.second = second;
        }

        public override void Write(Magic magic)
        {
            first.Write(magic);
            second.Write(magic);
        }

        public override void Write(byte[] array)
        {
            first.Write(array);
            second.Write(array);
        }

        public override void Write(long integer)
        {
            first.Write(integer);
            second.Write(integer);
        }

        public override void WriteCRLF()
        {
            first.WriteCRLF();
            second.WriteCRLF();
        }
    }
}