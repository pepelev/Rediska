using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rediska.Utils
{
    internal sealed class ConcatCollection<T> : IReadOnlyCollection<T>
    {
        private readonly IReadOnlyCollection<T> start;
        private readonly IReadOnlyCollection<T> end;

        public ConcatCollection(IReadOnlyCollection<T> start, IReadOnlyCollection<T> end)
        {
            this.start = start;
            this.end = end;
        }

        public IEnumerator<T> GetEnumerator() => start.Concat(end).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public int Count => start.Count + end.Count;
    }
}