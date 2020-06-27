namespace Rediska.Commands.Geo
{
    public sealed class GeospatialItem
    {
        public GeospatialItem(Key name, Location location)
        {
            Name = name;
            Location = location;
        }

        public Key Name { get; }
        public Location Location { get; }
    }
}