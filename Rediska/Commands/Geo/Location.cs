namespace Rediska.Commands.Geo
{
    using System;

    public struct Location : IEquatable<Location>
    {
        public const double MaxLongitudeAbsoluteValue = 180;
        public const double MaxLatitudeAbsoluteValue = 85.05112878;

        public Location(double longitude, double latitude)
        {
            if (Math.Abs(longitude) >= MaxLongitudeAbsoluteValue)
                throw new ArgumentException("Longitude must be between -180 and 180", nameof(longitude));

            if (Math.Abs(latitude) >= MaxLatitudeAbsoluteValue)
                throw new ArgumentException("Latitude must be between -85.05112878 and 85.05112878", nameof(latitude));

            Longitude = longitude;
            Latitude = latitude;
        }

        public double Longitude { get; }
        public double Latitude { get; }
        public bool Equals(Location other) => Longitude.Equals(other.Longitude) && Latitude.Equals(other.Latitude);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            return obj is Location other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Longitude.GetHashCode() * 397) ^ Latitude.GetHashCode();
            }
        }

        public static bool operator ==(Location left, Location right) => left.Equals(right);
        public static bool operator !=(Location left, Location right) => !left.Equals(right);
    }
}