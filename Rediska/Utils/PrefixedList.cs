using System.Collections;
using System.Collections.Generic;

namespace Rediska.Utils
{
    internal sealed class PrefixedList<T> : IReadOnlyList<T>
    {
        private readonly IReadOnlyList<T> list;
        private readonly T prefix;

        public PrefixedList(T prefix, IReadOnlyList<T> list)
        {
            this.prefix = prefix;
            this.list = list;
        }

        public IEnumerator<T> GetEnumerator()
        {
            yield return prefix;

            foreach (var item in list)
                yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public int Count => list.Count + 1;

        public T this[int index] => index == 0
            ? prefix
            : list[index - 1];
    }
}