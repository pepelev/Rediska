using System;

namespace Rediska.Tests.VerificationStates
{
    public sealed class LengthSpecifiedArray : State
    {
        private readonly State outerState;
        private readonly long length;

        public LengthSpecifiedArray(State outerState, long length)
        {
            this.outerState = outerState;
            this.length = length;
        }

        public override State Write(Magic magic)
        {
            throw new InvalidOperationException("CRLF expected");
        }

        public override State Write(byte[] array)
        {
            throw new InvalidOperationException("CRLF expected");
        }

        public override State Write(long integer)
        {
            throw new InvalidOperationException("CRLF expected");
        }

        public override State WriteCRLF()
        {
            return new ArrayElementStart(outerState, length);
        }
    }
}