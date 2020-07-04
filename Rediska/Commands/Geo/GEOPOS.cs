namespace Rediska.Commands.Geo
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class GEOPOS : Command<IReadOnlyList<Location?>>
    {
        private static readonly PlainBulkString name = new PlainBulkString("GEOPOS");

        private static readonly ListVisitor<Location?> responseStructure = new ListVisitor<Location?>(
            ArrayExpectation.Singleton,
            ArrayExpectation2.Singleton.Then(
                location => location switch
                {
                    {IsNull: true} => default(Location?),
                    {Count: 2} => new Location(
                        location[0].Accept(DoubleExpectation.Singleton),
                        location[1].Accept(DoubleExpectation.Singleton)
                    ),
                    _ => throw new ArgumentException("Expected null array or array of 2 elements")
                }
            )
        );

        private readonly Key key;
        private readonly IReadOnlyList<Key> members;

        public GEOPOS(Key key, params Key[] members)
            : this(key, members as IReadOnlyList<Key>)
        {
        }

        public GEOPOS(Key key, IReadOnlyList<Key> members)
        {
            this.key = key;
            this.members = members;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory)
        {
            return new ConcatList<BulkString>(
                new[]
                {
                    name,
                    key.ToBulkString()
                },
                new KeyList(members)
            );
        }

        public override Visitor<IReadOnlyList<Location?>> ResponseStructure => responseStructure;
    }
}