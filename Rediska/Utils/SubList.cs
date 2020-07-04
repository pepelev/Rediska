namespace Rediska.Utils
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal sealed class SubList<T> : IReadOnlyList<T>
    {
        private readonly IReadOnlyList<T> list;
        private readonly int start;

        public SubList(IReadOnlyList<T> list, int start, int count)
        {
            if (start < 0 || start >= list.Count)
                throw new ArgumentOutOfRangeException(nameof(start));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (start + count > list.Count)
                throw new ArgumentOutOfRangeException(nameof(count));

            this.list = list;
            this.start = start;
            Count = count;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public int Count { get; }
        public T this[int index] => list[index + start];
    }
}