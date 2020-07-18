namespace Rediska.Commands.Streams
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class XLEN : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("XLEN");
        private readonly Key key;

        public XLEN(Key key)
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