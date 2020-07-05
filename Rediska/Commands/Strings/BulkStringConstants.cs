namespace Rediska.Commands.Strings
{
    using Protocol;

    internal static class BulkStringConstants
    {
        public static PlainBulkString Zero { get; } = new PlainBulkString("0");
        public static PlainBulkString One { get; } = new PlainBulkString("1");
    }
}