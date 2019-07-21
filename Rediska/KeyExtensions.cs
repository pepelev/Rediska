using Rediska.Protocol.Requests;

namespace Rediska
{
    public static class KeyExtensions
    {
        public static BulkString ToBulkString(this Key key) => new BulkString(key.ToBytes());
    }
}