namespace Rediska.Commands.Sets
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class SCARD : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("SCARD");
        private readonly Key key;

        public SCARD(Key key)
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