namespace Rediska.Commands.Connection
{
    using System;

    public static partial class CLIENT
    {
        [Flags]
        public enum FileDescriptorEvents : byte
        {
            None = 0,
            Readable = 1,
            Writable = 2
        }
    }
}