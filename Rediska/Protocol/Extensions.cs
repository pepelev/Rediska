namespace Rediska.Protocol
{
    using System.Globalization;

    public static class Extensions
    {
        public static BulkString ToBulkString(this int value) => ((long)value).ToBulkString();
        public static BulkString ToBulkString(this uint value) => ((long) value).ToBulkString();

        public static BulkString ToBulkString(this long value) =>
            new PlainBulkString(value.ToString(CultureInfo.InvariantCulture));

        public static BulkString ToBulkString(this double value) =>
            new PlainBulkString(value.ToString(CultureInfo.InvariantCulture));
    }
}