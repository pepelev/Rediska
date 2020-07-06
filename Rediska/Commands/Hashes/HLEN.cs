namespace Rediska.Commands.Hashes
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class HLEN : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("HLEN");
        private readonly Key key;

        public HLEN(Key key)
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