using System.Collections;
using System.Collections.Generic;

namespace Rediska.Utils
{
    internal sealed class PrefixedCollection<T> : IReadOnlyCollection<T>
    {
        private readonly IReadOnlyCollection<T> collection;
        private readonly T prefix;

        public PrefixedCollection(T prefix, IReadOnlyCollection<T> collection)
        {
            this.prefix = prefix;
            this.collection = collection;
        }

        public IEnumerator<T> GetEnumerator()
        {
            yield return prefix;

            foreach (var item in collection)
                yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => collection.Count + 1;
    }
}