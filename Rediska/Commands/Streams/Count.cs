namespace Rediska.Commands.Streams
{
    using Protocol;
    using Array = System.Array;

    public abstract class Count
    {
        public static Count Unbound { get; } = new UnboundCount();
        public static implicit operator Count(uint value) => new Limit(value);
        public abstract BulkString[] Arguments(BulkStringFactory factory);

        private sealed class UnboundCount : Count
        {
            public override BulkString[] Arguments(BulkStringFactory factory) => Array.Empty<BulkString>();
        }
    }
}