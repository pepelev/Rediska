using System;
using Rediska.Tests.Checks;

namespace Rediska.Tests.VerificationStates
{
    public sealed class ArrayStart : State
    {
        private readonly State outerState;

        public ArrayStart(State outerState)
        {
            this.outerState = outerState;
        }

        public override State Write(Magic magic)
        {
            throw new InvalidOperationException("Array length expected");
        }

        public override State Write(byte[] array)
        {
            throw new InvalidOperationException("Array length expected");
        }

        public override State Write(long integer)
        {
            return new LengthSpecifiedArray(outerState, integer);
        }

        public override State WriteCRLF()
        {
            throw new InvalidOperationException("Array length expected");
        }

        public override bool IsTerminal => false;
    }
}