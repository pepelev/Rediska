namespace Rediska.Protocol
{
    using System.Globalization;

    public static class Extensions
    {
        public static BulkString ToBulkString(this long value) =>
            new PlainBulkString(value.ToString(CultureInfo.InvariantCulture));
    }
}