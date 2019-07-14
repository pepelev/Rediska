using System;

namespace Rediska.Tests.VerificationStates
{
    public sealed class Start : State
    {
        public override State Write(Magic magic)
        {
            if (magic == Magic.Array)
                return new ArrayStart(End.Singleton);

            throw new InvalidOperationException("Array magic expected");
        }

        public override State Write(byte[] array)
        {
            throw new InvalidOperationException("Array magic expected");
        }

        public override State Write(long integer)
        {
            throw new InvalidOperationException("Array magic expected");
        }

        public override State WriteCRLF()
        {
            throw new InvalidOperationException("Array magic expected");
        }
    }
}