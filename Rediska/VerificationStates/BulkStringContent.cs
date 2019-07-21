using System;
using Rediska.Protocol;

namespace Rediska.VerificationStates
{
    public sealed class BulkStringContent : State
    {
        private readonly long length;
        private readonly State outerState;
        private readonly long written;

        public BulkStringContent(State outerState, long length)
            : this(outerState, 0, length)
        {
        }

        private BulkStringContent(State outerState, long written, long length)
        {
            this.outerState = outerState;
            this.length = length;
            this.written = written;
        }

        public override bool IsTerminal => false;

        public override State Write(Magic magic)
        {
            if (AtEnd())
                throw new InvalidOperationException("CRLF expected");

            throw new InvalidOperationException("Bytes expected");
        }

        public override State Write(byte[] array)
        {
            var newWritten = written + array.Length;
            if (newWritten > length)
                throw new ArgumentException("Array is too large");

            return new BulkStringContent(outerState, newWritten, length);
        }

        public override State Write(long integer)
        {
            if (AtEnd())
                throw new InvalidOperationException("CRLF expected");

            throw new InvalidOperationException("Bytes expected");
        }

        public override State WriteCRLF()
        {
            if (AtEnd())
                return outerState;

            throw new InvalidOperationException($"You must write {length - written} extra bytes before CRLF");
        }

        private bool AtEnd() => written == length;
    }
}