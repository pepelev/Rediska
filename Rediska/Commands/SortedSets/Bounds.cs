namespace Rediska.Commands.SortedSets
{
    using Protocol;

    internal static class Bounds
    {
        public const string ExclusiveSign = "(";
        public const string InclusiveSign = "[";

        // todo move this to another place that more appropriate
        public static BulkString NegativeInfinity { get; } = new PlainBulkString(/*todo move inf to constant*/"-inf");
        public static BulkString PositiveInfinity { get; } = new PlainBulkString("+inf");
    }
}