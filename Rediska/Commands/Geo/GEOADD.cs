namespace Rediska.Commands.Geo
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class GEOADD : Command<AddResult>
    {
        private static readonly PlainBulkString name = new PlainBulkString("GEOADD");

        private static readonly Visitor<AddResult> responseStructure = IntegerExpectation.Singleton
            .Then(added => new AddResult(added));

        private readonly Key key;
        private readonly IReadOnlyList<GeospatialItem> items;

        public GEOADD(Key key, params GeospatialItem[] items)
            : this(key, items as IReadOnlyList<GeospatialItem>)
        {
        }

        public GEOADD(Key key, IReadOnlyList<GeospatialItem> items)
        {
            this.key = key;
            this.items = items;
        }

        public override DataType Request => new PlainArray(
            new ConcatList<DataType>(
                new DataType[]
                {
                    name,
                    key.ToBulkString()
                },
                new GeospatialItemList(items)
            )
        );

        public override Visitor<AddResult> ResponseStructure => responseStructure;
    }
}