using Rediska.Protocol;

namespace Rediska
{
    public static class KeyExtensions
    {
        public static BulkString ToBulkString(this Key key) => new PlainBulkString(key.ToBytes());
    }
}