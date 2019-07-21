using Rediska.Tests.Protocol.Requests;

namespace Rediska.Tests
{
    public static class KeyExtensions
    {
        public static BulkString ToBulkString(this Key key) => new BulkString(key.ToBytes());
    }
}