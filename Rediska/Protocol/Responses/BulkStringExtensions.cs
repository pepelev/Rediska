using System.IO;

namespace Rediska.Protocol.Responses
{
    public static class BulkStringExtensions
    {
        public static byte[] ToBytes(this BulkString @string)
        {
            if (@string.IsNull)
                return null;

            using (var stream = new MemoryStream())
            {
                @string.Write(stream);
                return stream.ToArray();
            }
        }
    }
}