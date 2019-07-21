using Rediska.Protocol;

namespace Rediska.VerificationStates
{
    public abstract class State
    {
        public abstract State Write(Magic magic);
        public abstract State Write(byte[] array);
        public abstract State Write(long integer);
        public abstract State WriteCRLF();
        public abstract bool IsTerminal { get; }
    }
}