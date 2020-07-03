namespace Rediska.Utils
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal sealed class PairsList<T> : IReadOnlyList<(T Left, T Right)>
    {
        private readonly IReadOnlyList<T> list;

        public PairsList(IReadOnlyList<T> list)
        {
            if (list.Count % 2 != 0)
            {
                throw new ArgumentException("Must contain even number of elements", nameof(list));
            }

            this.list = list;
        }

        public IEnumerator<(T Left, T Right)> GetEnumerator()
        {
            var count = Count;
            for (var i = 0; i < count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public int Count => list.Count / 2;
        public (T Left, T Right) this[int index] => (list[index * 2], list[index * 2 + 1]);
    }
}