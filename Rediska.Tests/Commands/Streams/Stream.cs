namespace Rediska.Tests.Commands.Streams
{
    public sealed class Stream
    {
        public Stream(Key key, params Entry[] entries)
        {
            Key = key;
            Entries = entries;
        }

        public Key Key { get; }
        public Entry[] Entries { get; }
    }
}