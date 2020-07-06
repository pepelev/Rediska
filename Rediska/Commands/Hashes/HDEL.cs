namespace Rediska.Commands.Hashes
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class HDEL : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("HDEL");
        private readonly Key key;
        private readonly IReadOnlyList<BulkString> fields;

        public HDEL(Key key, IReadOnlyList<BulkString> fields)
        {
            this.key = key;
            this.fields = fields;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new ConcatList<BulkString>(
            new[]
            {
                name,
                key.ToBulkString(factory)
            },
            fields
        );

        public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;
    }
}