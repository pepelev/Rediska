namespace Rediska.Commands
{
    using System.Collections;
    using System.Collections.Generic;
    using Protocol;

    public sealed class ScanResult : IReadOnlyList<BulkString>
    {
        private readonly IReadOnlyList<BulkString> members;

        public ScanResult(Cursor next, IReadOnlyList<BulkString> members)
        {
            this.members = members;
            Next = next;
        }

        public IEnumerator<BulkString> GetEnumerator() => members.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public int Count => members.Count;
        public BulkString this[int index] => members[index];
        public Cursor Next { get; }
        public bool ScanTerminated => Next == Cursor.Start;
    }
}