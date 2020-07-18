namespace Rediska.Commands.Streams
{
    using System.Collections;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class Entries : IReadOnlyList<Entry>
    {
        private readonly Array reply;

        public Entries(Array reply)
        {
            this.reply = reply;
        }

        public Key Stream => new Key.BulkString(
            reply[0].Accept(BulkStringExpectation.Singleton)
        );

        private IReadOnlyList<Entry> Content => new ProjectingReadOnlyList<Array, Entry>(
            reply[1].Accept(CompositeVisitors.ArrayList),
            array => new Entry(array)
        );

        public IEnumerator<Entry> GetEnumerator() => Content.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public int Count => reply.Count;
        public Entry this[int index] => Content[index];
    }
}