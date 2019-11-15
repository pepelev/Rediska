namespace Rediska.Commands.Keys
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class KEYS : Command<IReadOnlyList<BulkString>>
    {
        private static readonly PlainBulkString name = new PlainBulkString("KEYS");
        private readonly string pattern;

        public KEYS(string pattern)
        {
            this.pattern = pattern;
        }

        public override DataType Request => new PlainArray(
            name,
            new PlainBulkString(pattern)
        );

        public override Visitor<IReadOnlyList<BulkString>> ResponseStructure => CompositeVisitors.BulkStringList;
    }
}