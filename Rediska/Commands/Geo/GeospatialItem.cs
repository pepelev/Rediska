namespace Rediska.Commands.Geo
{
    using Protocol;

    public sealed class GeospatialItem
    {
        public GeospatialItem(BulkString name, Location location)
        {
            Name = name;
            Location = location;
        }

        public BulkString Name { get; }
        public Location Location { get; }
    }
}