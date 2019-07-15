using System;
using Rediska.Tests.Checks;

namespace Rediska.Tests.VerificationStates
{
    public sealed class LengthSpecifiedArray : State
    {
        private readonly long length;
        private readonly State outerState;

        public LengthSpecifiedArray(State outerState, long length)
        {
            this.outerState = outerState;
            this.length = length;
        }

        public override bool IsTerminal => false;
        public override State Write(Magic magic) => throw new InvalidOperationException("CRLF expected");
        public override State Write(byte[] array) => throw new InvalidOperationException("CRLF expected");
        public override State Write(long integer) => throw new InvalidOperationException("CRLF expected");

        public override State WriteCRLF() => length <= 0
            ? outerState
            : new ArrayElementStart(outerState, length);
    }
}