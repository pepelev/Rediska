namespace Rediska.Commands.Lists
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class LSET : Command<None>
    {
        private static readonly PlainBulkString name = new PlainBulkString("LSET");
        private readonly Key key;
        private readonly Index index;
        private readonly BulkString element;

        public LSET(Key key, Index index, BulkString element)
        {
            this.key = key;
            this.index = index;
            this.element = element;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory),
            index.ToBulkString(factory),
            element
        };

        public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
    }
}