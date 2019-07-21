using System;
using Rediska.Protocol;

namespace Rediska.VerificationStates
{
    public sealed class ArrayElementStart : State
    {
        private readonly State outerState;
        private readonly long length;
        private readonly long items;

        public ArrayElementStart(State outerState, long length)
            : this(outerState, 0, length)
        {
        }

        private ArrayElementStart(State outerState, long items, long length)
        {
            this.outerState = outerState;
            this.items = items;
            this.length = length;
        }

        public override State Write(Magic magic)
        {
            if (AtEnd())
                throw new InvalidOperationException("No more elements allowed for this array");

            if (magic == Magic.Array)
                return new ArrayStart(Next);

            if (magic == Magic.Integer)
                return new IntegerStart(Next);

            if (magic == Magic.BulkString)
                return new BulkStringStart(Next);

            throw new ArgumentException("Only Array, Integer and BulkString are allowed");
        }

        public override State Write(byte[] array) => throw new InvalidOperationException("You must write magic at first");
        public override State Write(long integer) => throw new InvalidOperationException("You must write magic at first");
        public override State WriteCRLF() => throw new InvalidOperationException("You must write magic at first");
        public override bool IsTerminal => false;
        private bool AtEnd() => items == length;

        private State Next => items + 1 == length
            ? outerState
            : new ArrayElementStart(outerState, items + 1, length);
    }
}