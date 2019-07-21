using System;
using Rediska.Protocol;

namespace Rediska.VerificationStates
{
    public sealed class BulkStringStart : State
    {
        private readonly State outerState;

        public BulkStringStart(State outerState)
        {
            this.outerState = outerState;
        }

        public override State Write(Magic magic) => throw new InvalidOperationException("BulkString length expected");
        public override State Write(byte[] array) => throw new InvalidOperationException("BulkString length expected");
        public override State Write(long integer) => new BulkStringLengthEnd(outerState, integer);
        public override State WriteCRLF() => throw new InvalidOperationException("BulkString length expected");
        public override bool IsTerminal => false;
    }
}