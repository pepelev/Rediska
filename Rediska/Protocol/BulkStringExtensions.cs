using System.IO;

namespace Rediska.Protocol
{
    public static class BulkStringExtensions
    {
        public static byte[] ToBytes(this BulkString @string)
        {
            if (@string.IsNull)
                return null;

            using (var stream = new MemoryStream())
            {
                @string.WriteContent(stream);
                return stream.ToArray();
            }
        }
    }
}