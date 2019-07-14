using System.IO;

namespace Rediska.Tests
{
    public static class BulkStringResponseExtensions
    {
        public static byte[] ToArray(this BulkStringResponse response)
        {
            using (var stream = new MemoryStream())
            {
                response.Write(stream);
                return stream.ToArray();
            }
        }
    }
}