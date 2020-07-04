using Rediska.Protocol;

namespace Rediska
{
    public static class KeyExtensions
    {
        // todo move to Key for performance reasons
        public static BulkString ToBulkString(this Key key) => new PlainBulkString(key.ToBytes());
    }
}