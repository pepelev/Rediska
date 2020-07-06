namespace Rediska.Commands.Hashes
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class HGET : Command<BulkString>
    {
        private static readonly PlainBulkString name = new PlainBulkString("HGET");
        private readonly Key key;
        private readonly BulkString field;

        public HGET(Key key, BulkString field)
        {
            this.key = key;
            this.field = field;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory)
        };

        public override Visitor<BulkString> ResponseStructure => BulkStringExpectation.Singleton;
    }
}