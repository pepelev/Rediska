namespace Rediska.Utils
{
    using System.Collections;
    using System.Collections.Generic;
    using Commands.Geo;
    using Protocol;

    internal sealed class GeospatialItemList : IReadOnlyList<DataType>
    {
        private readonly IReadOnlyList<GeospatialItem> items;

        public GeospatialItemList(IReadOnlyList<GeospatialItem> items)
        {
            this.items = items;
        }

        public IEnumerator<DataType> GetEnumerator()
        {
            foreach (var item in items)
            {
                yield return item.Location.Longitude.ToBulkString();
                yield return item.Location.Latitude.ToBulkString();
                yield return item.Name.ToBulkString();
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public int Count => items.Count * 3;

        public DataType this[int index]
        {
            get
            {
                var itemIndex = index / 3;
                return (index % 3) switch
                {
                    0 => items[itemIndex].Location.Longitude.ToBulkString(),
                    1 => items[itemIndex].Location.Latitude.ToBulkString(),
                    _ => items[itemIndex].Name.ToBulkString()
                };
            }
        }
    }
}