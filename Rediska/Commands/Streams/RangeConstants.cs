namespace Rediska.Commands.Streams
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    internal static class RangeConstants
    {
        public static PlainBulkString Minimum { get; } = new PlainBulkString("-");
        public static PlainBulkString Maximum { get; } = new PlainBulkString("+");

        public static Visitor<IReadOnlyList<Entry>> ResponseStructure { get; } = CompositeVisitors.ArrayList
            .Then(
                list => new ProjectingReadOnlyList<Array, Entry>(
                    list,
                    array => new Entry(array)
                ) as IReadOnlyList<Entry>
            );
    }
}