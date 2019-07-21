namespace Rediska.Commands.Hashes
{
    public abstract class HashEntry
    {
        public abstract Key Key { get; }
        public abstract BulkString Value { get; }
    }
}