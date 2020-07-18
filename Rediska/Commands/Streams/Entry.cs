namespace Rediska.Commands.Streams
{
    using System.Collections;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class Entry : IReadOnlyList<(BulkString Field, BulkString Value)>
    {
        private readonly Array reply;

        public Entry(Array reply)
        {
            this.reply = reply;
        }

        public Id Id => Id.Parse(
            reply[0].Accept(BulkStringExpectation.Singleton).ToString()
        );

        private IReadOnlyList<(BulkString Field, BulkString Value)> Members => new PairsList<BulkString>(
            reply[1].Accept(CompositeVisitors.BulkStringList)
        );

        public IEnumerator<(BulkString Field, BulkString Value)> GetEnumerator() => Members.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public int Count => Members.Count;
        public (BulkString Field, BulkString Value) this[int index] => Members[index];
    }
}