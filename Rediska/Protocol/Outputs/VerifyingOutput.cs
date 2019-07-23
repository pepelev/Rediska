using Rediska.VerificationStates;

namespace Rediska.Protocol.Outputs
{
    public sealed class VerifyingOutput : Output
    {
        private State state = Start.Singleton;

        public override void Write(Magic magic)
        {
            state = state.Write(magic);
        }

        public override void Write(byte[] array)
        {
            state = state.Write(array);
        }

        public override void Write(long integer)
        {
            state = state.Write(integer);
        }

        public override void WriteCRLF()
        {
            state = state.WriteCRLF();
        }

        public bool Completed() => state.IsTerminal;
    }
}