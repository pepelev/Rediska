namespace Rediska.Commands.Keys
{
    public sealed partial class SORT
    {
        public readonly struct Limit
        {
            public Limit(long offset, long count)
            {
                Offset = offset;
                Count = count;
            }

            public long Offset { get; }
            public long Count { get; }
        }
    }
}