using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rediska.Protocol;

namespace Rediska.Utils
{
    internal sealed class KeyList : IReadOnlyList<BulkString>
    {
        private readonly IReadOnlyList<Key> keys;

        public KeyList(IReadOnlyList<Key> keys)
        {
            this.keys = keys;
        }

        public IEnumerator<BulkString> GetEnumerator() => keys
            .Select(key => key.ToBulkString())
            .GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => keys.Count;
        public BulkString this[int index] => keys[index].ToBulkString();
    }
}