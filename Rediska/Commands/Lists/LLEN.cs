namespace Rediska.Commands.Lists
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class LLEN : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("LLEN");
        private readonly Key key;

        public LLEN(Key key)
        {
            this.key = key;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory)
        };

        public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;
    }
}