namespace Rediska.Utils
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Commands;
    using Protocol;

    internal sealed class KeyList : IReadOnlyList<BulkString>
    {
        private readonly BulkStringFactory factory;
        private readonly IReadOnlyList<Key> keys;

        public KeyList(BulkStringFactory factory, IReadOnlyList<Key> keys)
        {
            this.keys = keys;
            this.factory = factory;
        }

        public IEnumerator<BulkString> GetEnumerator() => keys
            .Select(key => key.ToBulkString(factory))
            .GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public int Count => keys.Count;
        public BulkString this[int index] => keys[index].ToBulkString(factory);
    }
}