namespace Rediska.Protocol
{
    using System.Globalization;
    using Commands;

    public static class Extensions
    {
        public static BulkString ToBulkString(this long value) =>
            new PlainBulkString(value.ToString(CultureInfo.InvariantCulture));

        public static BulkString ToBulkString(this double value) =>
            new PlainBulkString(value.ToString(CultureInfo.InvariantCulture));
    }
}