using System;
using Rediska.Protocol;

namespace Rediska.VerificationStates
{
    public sealed class Start : State
    {
        public static Start Singleton { get; } = new Start();

        public override State Write(Magic magic)
        {
            if (magic == Magic.Array)
                return new ArrayStart(End.Singleton);

            throw new InvalidOperationException("Array magic expected");
        }

        public override State Write(byte[] array) => throw new InvalidOperationException("Array magic expected");
        public override State Write(long integer) => throw new InvalidOperationException("Array magic expected");
        public override State WriteCRLF() => throw new InvalidOperationException("Array magic expected");
        public override bool IsTerminal => false;
    }
}