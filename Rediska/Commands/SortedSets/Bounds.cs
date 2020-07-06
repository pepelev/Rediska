namespace Rediska.Commands.SortedSets
{
    using Protocol;

    internal static class Bounds
    {
        public const string ExclusiveSign = "(";
        public static BulkString NegativeInfinity { get; } = new PlainBulkString("-inf");
        public static BulkString PositiveInfinity { get; } = new PlainBulkString("+inf");
    }
}