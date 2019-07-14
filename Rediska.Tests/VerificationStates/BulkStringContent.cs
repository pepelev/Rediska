using System;

namespace Rediska.Tests.VerificationStates
{
    public sealed class BulkStringContent : State
    {
        private readonly State outerState;
        private readonly long written;
        private readonly long length;

        public BulkStringContent(State outerState, long integer)
            : this(outerState, 0, integer)
        {
        }

        private BulkStringContent(State outerState, long written, long length)
        {
            this.outerState = outerState;
            this.length = length;
            this.written = written;
        }

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