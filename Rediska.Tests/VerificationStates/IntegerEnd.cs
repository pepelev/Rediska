using System;

namespace Rediska.Tests.VerificationStates
{
    public sealed class IntegerEnd : State
    {
        private readonly State outerState;

        public IntegerEnd(State outerState)
        {
            this.outerState = outerState;
        }

        public override State Write(Magic magic) => throw new InvalidOperationException("CRLF expected");
        public override State Write(byte[] array) => throw new InvalidOperationException("CRLF expected");
        public override State Write(long integer) => throw new InvalidOperationException("CRLF expected");
        public override State WriteCRLF() => outerState;
    }
}