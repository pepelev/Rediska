namespace Rediska.Commands.Hashes
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class HMGET : Command<IReadOnlyList<BulkString>>
    {
        private static readonly PlainBulkString name = new PlainBulkString("HMGET");
        private readonly Key key;
        private readonly IReadOnlyList<BulkString> fields;

        public HMGET(Key key, params BulkString[] fields)
            : this(key, fields as IReadOnlyList<BulkString>)
        {
        }

        public HMGET(Key key, IReadOnlyList<BulkString> fields)
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

        public override Visitor<IReadOnlyList<BulkString>> ResponseStructure => CompositeVisitors.BulkStringList;
    }
}