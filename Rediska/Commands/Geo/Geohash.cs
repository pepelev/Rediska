namespace Rediska.Commands.Geo
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class GEOHASH : Command<IReadOnlyList<(Key Member, Geohash Location)>>
    {
        private static readonly PlainBulkString name = new PlainBulkString("GEOHASH");
        private readonly Key key;
        private readonly IReadOnlyList<Key> members;

        public GEOHASH(Key key, params Key[] members)
            : this(key, members as IReadOnlyList<Key>)
        {
        }

        public GEOHASH(Key key, IReadOnlyList<Key> members)
        {
            this.key = key;
            this.members = members;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new ConcatList<BulkString>(
            new[]
            {
                name,
                key.ToBulkString(factory)
            },
            new KeyList(factory, members)
        );

        public override Visitor<IReadOnlyList<(Key Member, Geohash Location)>> ResponseStructure => CompositeVisitors.BulkStringList
            .Then(
                response => new PairwiseReadOnlyList<Key, Geohash>(
                    members,
                    new ProjectingReadOnlyList<BulkString, Geohash>(
                        response,
                        bulkString => new Geohash(bulkString.ToBytes())
                    )
                ) as IReadOnlyList<(Key Member, Geohash Location)>
            );
    }

    public readonly struct Geohash
    {
        private const int expectedLength = 11;
        private readonly byte[] content;

        public Geohash(byte[] content)
        {
            if (content.Length != expectedLength)
                throw new ArgumentException("Expected array with 11 items");

            this.content = content;
        }

        public override string ToString() => Encoding.UTF8.GetString(content);
    }
}