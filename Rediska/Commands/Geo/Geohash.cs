namespace Rediska.Commands.Geo
{
    using System;
    using System.Text;

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