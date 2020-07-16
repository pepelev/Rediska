namespace Rediska.Commands.Connection
{
    using System;
    using System.Globalization;

    public static partial class CLIENT
    {
        public readonly struct Flag : IEquatable<Flag>
        {
            public static Flag ConnectionClosingAsSoonAsPossible => new Flag('A');
            public static Flag InBlockedOperation => new Flag('b');
            public static Flag ConnectionClosingAfterWritingEntireReply => new Flag('c');
            public static Flag ExecWillFail => new Flag('d');
            public static Flag WaitingForVmIo => new Flag('i');
            public static Flag Master => new Flag('M');
            public static Flag NoSpecificFlagSet => new Flag('N');
            public static Flag MonitorMode => new Flag('O');
            public static Flag PubSubMode => new Flag('P');
            public static Flag ReadOnlyAgainstCluster => new Flag('r');
            public static Flag Replica => new Flag('S');
            public static Flag Unblocked => new Flag('u');
            public static Flag UnixDomainSocket => new Flag('U');
            public static Flag TransactionMode => new Flag('x');
            private readonly char letter;

            public Flag(char letter)
            {
                this.letter = letter;
            }

            public bool Equals(Flag other) => letter == other.letter;
            public static bool operator ==(Flag left, Flag right) => left.Equals(right);
            public static bool operator !=(Flag left, Flag right) => !left.Equals(right);

            public override string ToString() => letter switch
            {
                'A' => "A: connection to be closed ASAP",
                'b' => "b: the client is waiting in a blocking operation",
                'c' => "c: connection to be closed after writing entire reply",
                'd' => "d: a watched keys has been modified - EXEC will fail",
                'i' => "i: the client is waiting for a VM I/O (deprecated)",
                'M' => "M: the client is a master",
                'N' => "N: no specific flag set",
                'O' => "O: the client is a client in MONITOR mode",
                'P' => "P: the client is a Pub/Sub subscriber",
                'r' => "r: the client is in readonly mode against a cluster node",
                'S' => "S: the client is a replica node connection to this instance",
                'u' => "u: the client is unblocked",
                'U' => "U: the client is connected via a Unix domain socket",
                'x' => "x: the client is in a MULTI/EXEC context",
                { } @char => @char.ToString(CultureInfo.InvariantCulture)
            };

            public override bool Equals(object obj) => obj is Flag other && Equals(other);
            public override int GetHashCode() => letter.GetHashCode();
        }
    }
}