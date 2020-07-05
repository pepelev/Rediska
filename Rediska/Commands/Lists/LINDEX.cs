namespace Rediska.Commands.Lists
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class LINDEX : Command<BulkString>
    {
        private static readonly PlainBulkString name = new PlainBulkString("LINDEX");
        private readonly Key key;
        private readonly Index index;

        public LINDEX(Key key, Index index)
        {
            this.key = key;
            this.index = index;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory),
            index.ToBulkString(factory)
        };

        public override Visitor<BulkString> ResponseStructure => BulkStringExpectation.Singleton;
    }
}