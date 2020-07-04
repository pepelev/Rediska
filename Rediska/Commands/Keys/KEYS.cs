namespace Rediska.Commands.Keys
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class KEYS : Command<IReadOnlyList<Key>>
    {
        private static readonly PlainBulkString name = new PlainBulkString("KEYS");
        private readonly string pattern;

        public KEYS(string pattern)
        {
            this.pattern = pattern;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            factory.Utf8(pattern)
        };

        public override Visitor<IReadOnlyList<Key>> ResponseStructure => CompositeVisitors.KeyList;
    }
}