using System;

namespace Rediska.Tests.VerificationStates
{
    public sealed class End : State
    {
        public static End Singleton { get; } = new End();

        public override State Write(Magic magic) => throw new InvalidOperationException("Element already completed");
        public override State Write(byte[] array) => throw new InvalidOperationException("Element already completed");
        public override State Write(long integer) => throw new InvalidOperationException("Element already completed");
        public override State WriteCRLF() => throw new InvalidOperationException("Element already completed");
    }
}