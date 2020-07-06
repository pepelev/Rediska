using Rediska.Protocol;

namespace Rediska
{
    using System;

    public static class KeyExtensions
    {
        // todo move to Key for performance reasons
        [Obsolete]
        public static BulkString ToBulkString(this Key key) => new PlainBulkString(key.ToBytes());
    }
}