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
        private readonly IReadOnlyList<Key> fields;

        public HMGET(Key key, params Key[] fields)
            : this(key, fields as IReadOnlyList<Key>)
        {
        }

        public HMGET(Key key, IReadOnlyList<Key> fields)
        {
            this.key = key;
            this.fields = fields;
        }

        public override DataType Request => new PlainArray(
            new ConcatList<DataType>(
                new DataType[]
                {
                    name,
                    key.ToBulkString()
                },
                new KeyList(fields)
            )
        );

        public override Visitor<IReadOnlyList<BulkString>> ResponseStructure => CompositeVisitors.BulkStringList;
    }
}