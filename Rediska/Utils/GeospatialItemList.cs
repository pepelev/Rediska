namespace Rediska.Utils
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
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
                yield return new PlainBulkString(
                    item.Location.Longitude.ToString(CultureInfo.InvariantCulture)
                );

                yield return new PlainBulkString(
                    item.Location.Latitude.ToString(CultureInfo.InvariantCulture)
                );

                yield return item.Name;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public int Count => items.Count * 3;

        public DataType this[int index]
        {
            get
            {
                var itemIndex = index / 3;
                switch (index % 3)
                {
                    case 0:
                        return new PlainBulkString(
                            items[itemIndex].Location.Longitude.ToString(CultureInfo.InvariantCulture)
                        );
                    case 1:
                        return new PlainBulkString(
                            items[itemIndex].Location.Latitude.ToString(CultureInfo.InvariantCulture)
                        );
                    default:
                        return items[itemIndex].Name;
                }
            }
        }
    }
}