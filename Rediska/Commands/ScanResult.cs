using System.Collections;
using System.Collections.Generic;
using Rediska.Protocol;
using Rediska.Protocol.Visitors;

namespace Rediska.Commands
{
    public sealed class ScanResult : IReadOnlyList<BulkString>
    {
        private static readonly ListVisitor<BulkString> responseStructure = new ListVisitor<BulkString>(
            ArrayExpectation.Singleton,
            BulkStringExpectation.Singleton
        );

        private readonly IReadOnlyList<BulkString> members;

        public ScanResult(Cursor next, IReadOnlyList<BulkString> members)
        {
            this.members = members;
            Next = next;
        }

        public Cursor Next { get; }
        public bool ScanTerminated => Next == Cursor.Start;
        public IEnumerator<BulkString> GetEnumerator() => members.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public int Count => members.Count;
        public BulkString this[int index] => members[index];
    }
}