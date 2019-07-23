using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rediska.Utils
{
    internal sealed class ConcatList<T> : IReadOnlyList<T>
    {
        private readonly IReadOnlyList<T> start;
        private readonly IReadOnlyList<T> end;

        public ConcatList(IReadOnlyList<T> start, IReadOnlyList<T> end)
        {
            this.start = start;
            this.end = end;
        }

        public IEnumerator<T> GetEnumerator() => start.Concat(end).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public int Count => start.Count + end.Count;

        public T this[int index] => index < start.Count
            ? start[index]
            : end[index - start.Count];
    }
}