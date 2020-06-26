namespace Rediska.Cluster
{
    public sealed class HashSlotMap<T> where T : class
    {
        private readonly T[] map = new T[1024 * 16];
    }
}