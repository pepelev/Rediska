namespace Rediska.Commands.Geo
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class GEOADD : Command<GEOADD.Response>
    {
        private static readonly PlainBulkString name = new PlainBulkString("GEOADD");

        private static readonly Visitor<Response> responseStructure = IntegerExpectation.Singleton
            .Then(added => new Response(added));

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

        public override IEnumerable<BulkString> Request(BulkStringFactory factory)
        {
            yield return name;
            yield return key.ToBulkString();
            foreach (var item in items)
            {
                yield return factory.Create(item.Location.Longitude);
                yield return factory.Create(item.Location.Latitude);
                yield return item.Name.ToBulkString();
            }
        }

        public override Visitor<Response> ResponseStructure => responseStructure;

        public readonly struct Response
        {
            public Response(long itemsAdded)
            {
                ItemsAdded = itemsAdded;
            }

            public long ItemsAdded { get; }
            public override string ToString() => $"{nameof(ItemsAdded)}: {ItemsAdded}";
        }
    }
}