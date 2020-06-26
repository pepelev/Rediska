namespace Rediska.Commands
{
    using System.Collections;
    using System.Collections.Generic;

    public sealed class ScanResult<T> : IReadOnlyList<T>
    {
        private readonly IReadOnlyList<T> members;

        public ScanResult(Cursor next, IReadOnlyList<T> members)
        {
            this.members = members;
            Next = next;
        }

        public IEnumerator<T> GetEnumerator() => members.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public int Count => members.Count;
        public T this[int index] => members[index];
        public Cursor Next { get; }
        public bool ScanTerminated => Next == Cursor.Start;
    }
}