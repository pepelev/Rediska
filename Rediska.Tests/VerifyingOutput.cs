using System;
using System.Collections.Generic;
using System.Linq;

namespace Rediska.Tests
{
    public sealed class VerifyingOutput : Output
    {
        private readonly Stack<State> states = new Stack<State>(new[] {State.Start});
        
        private enum State : byte
        {
            Start,

            ArrayStart,
            ArrayLength,
            ArrayElements,

            IntegerStart,
            IntegerEnd,

            BulkStringStart,
            BulkStringLength,
            BulkStringLengthEnd,
            BulkStringLengthContent,
        }

        public override void Write(Magic magic)
        {
            if (states.SequenceEqual(new[] {State.Start}) && magic == Magic.Array)
            {
                states.ReplaceTop(State.ArrayStart);
                return;
            }
            if (states.Peek() == State.ArrayElements)
            {
                if (magic == Magic.Integer)
                {
                    states.Push(State.IntegerStart);
                    return;
                }

                if (magic == Magic.BulkString)
                {
                    states.Push(State.BulkStringStart);
                    return;
                }
            }

            throw new InvalidOperationException("Only array magic may be at start");
        }

        public override void Write(byte[] array)
        {

        }

        public override void Write(long integer)
        {
            if (states.Peek() == State.ArrayStart)
            {
                states.ReplaceTop(State.ArrayLength);
                return;
            }

            if (states.Peek() == State.IntegerStart)
            {
                states.ReplaceTop(State.IntegerEnd);
                return;
            }

            throw new InvalidOperationException("Integer used only to write array length");
        }

        public override void WriteCRLF()
        {
            if (states.Peek() == State.ArrayLength)
            {
                states.ReplaceTop(State.ArrayElements);
                return;
            }

            if (states.Peek() == State.ArrayElements || states.Peek() == State.IntegerEnd)
            {
                states.Pop();
                return;
            }

            throw new InvalidOperationException("Not at end");
        }

        public bool VerifyCompleted() => states.Count == 0;
    }
}