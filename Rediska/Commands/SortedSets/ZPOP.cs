namespace Rediska.Commands.SortedSets
{
    using Protocol;

    internal static class ZPOP
    {
        public static BulkString[] Request(BulkString name, Key key, long count, BulkStringFactory factory) =>
            count == 1
                ? new[]
                {
                    name,
                    key.ToBulkString(factory)
                }
                : new[]
                {
                    name,
                    key.ToBulkString(factory),
                    factory.Create(count)
                };
    }
}