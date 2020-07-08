namespace Rediska.Protocol
{
    using System.Globalization;
    using Commands.SortedSets;

    public static class Extensions
    {
        public static BulkString ToBulkString(this int value) => ((long)value).ToBulkString();
        public static BulkString ToBulkString(this uint value) => ((long) value).ToBulkString();

        public static BulkString ToBulkString(this long value) =>
            new PlainBulkString(value.ToString(CultureInfo.InvariantCulture));

        public static BulkString ToBulkString(this double value)
        {
            if (double.IsPositiveInfinity(value))
                return Bounds.PositiveInfinity;
            
            if (double.IsNegativeInfinity(value))
                return Bounds.NegativeInfinity;
            
            return new PlainBulkString(value.ToString(CultureInfo.InvariantCulture));
        }
    }
}