using System;
using Rediska.Protocol;

namespace Rediska.VerificationStates
{
    public sealed class BulkStringLengthEnd : State
    {
        private readonly long length;
        private readonly State outerState;

        public BulkStringLengthEnd(State outerState, long length)
        {
            this.outerState = outerState;
            this.length = length;
        }

        public override State Write(Magic magic) => throw new InvalidOperationException("CRLF expected");
        public override State Write(byte[] array) => throw new InvalidOperationException("CRLF expected");
        public override State Write(long integer) => throw new InvalidOperationException("CRLF expected");
        public override State WriteCRLF() => new BulkStringContent(outerState, length);
        public override bool IsTerminal => false;
    }
}