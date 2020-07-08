using System.IO;

namespace Rediska.Protocol
{
    using System;
    using System.Globalization;
    using System.Text;
    using Commands.SortedSets;

    public static class BulkStringExtensions
    {
        public static byte[] ToBytes(this BulkString @string)
        {
            if (@string.IsNull)
                return null;

            using var stream = new MemoryStream();
            @string.WriteContent(stream);
            return stream.ToArray();
        }

        public static double? ToDoubleOrNull(this BulkString @string)
        {
            if (@string.IsNull)
                return null;

            if (ReferenceEquals(@string, Bounds.PositiveInfinity))
                return double.PositiveInfinity;

            if (ReferenceEquals(@string, Bounds.NegativeInfinity))
                return double.NegativeInfinity;

            using var stream = new MemoryStream();
            @string.WriteContent(stream);
            var doubleString = Encoding.UTF8.GetString(
                stream.GetBuffer(),
                0,
                checked((int) stream.Length)
            );

            // todo move to constants
            if (doubleString == "+inf")
                return double.PositiveInfinity;
            if (doubleString == "-inf")
                return double.NegativeInfinity;

            return double.Parse(doubleString, NumberStyles.Float, CultureInfo.InvariantCulture);
        }

        public static double ToDouble(this BulkString @string)
        {
            if (@string.ToDoubleOrNull() is {} result)
            {
                return result;
            }

            throw new ArgumentException("Bulk string is null", nameof(@string));
        }
    }
}