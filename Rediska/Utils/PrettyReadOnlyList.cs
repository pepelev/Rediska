namespace Rediska.Utils
{
    using System.Collections;
    using System.Collections.Generic;

    internal sealed class PrettyReadOnlyList<T> : IReadOnlyList<T>
    {
        private readonly IReadOnlyList<T> list;

        public PrettyReadOnlyList(IReadOnlyList<T> list)
        {
            this.list = list;
        }

        public IEnumerator<T> GetEnumerator() => list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();
        public int Count => list.Count;
        public T this[int index] => list[index];
        public override string ToString() => string.Join(", ", list);
    }
}