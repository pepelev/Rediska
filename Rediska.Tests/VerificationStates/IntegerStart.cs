using System;

namespace Rediska.Tests.VerificationStates
{
    public sealed class IntegerStart : State
    {
        private readonly State outerState;

        public IntegerStart(State outerState)
        {
            this.outerState = outerState;
        }

        public override State Write(Magic magic) => throw new InvalidOperationException("Number expected");
        public override State Write(byte[] array) => throw new InvalidOperationException("Number expected");
        public override State Write(long integer) => new IntegerEnd(outerState);
        public override State WriteCRLF() => throw new InvalidOperationException("Number expected");
    }
}