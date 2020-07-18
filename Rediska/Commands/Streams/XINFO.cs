namespace Rediska.Commands.Streams
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Protocol;
    using Protocol.Visitors;
    using Utils;
    using Array = Protocol.Array;

    public static partial class XINFO
    {
        private static readonly PlainBulkString name = new PlainBulkString("XINFO");
    }
}