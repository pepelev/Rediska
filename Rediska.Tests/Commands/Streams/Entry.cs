namespace Rediska.Tests.Commands.Streams
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Protocol;

    public sealed class Entry : IReadOnlyList<(BulkString Field, BulkString Value)>
    {
        private readonly (BulkString Field, BulkString Value)[] content;

        public Entry(params (BulkString Field, BulkString Value)[] content)
        {
            this.content = content;
        }

        public IEnumerator<(BulkString Field, BulkString Value)> GetEnumerator() => content.AsEnumerable().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => content.GetEnumerator();
        public int Count => content.Length;
        public (BulkString Field, BulkString Value) this[int index] => content[index];
    }
}