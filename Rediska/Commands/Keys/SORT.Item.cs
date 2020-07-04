namespace Rediska.Commands.Keys
{
    using System.Collections;
    using System.Collections.Generic;
    using Protocol;

    public sealed partial class SORT
    {
        public readonly struct Item : IReadOnlyList<BulkString>
        {
            private readonly IReadOnlyList<BulkString> list;

            public Item(IReadOnlyList<BulkString> list)
            {
                this.list = list;
            }

            public IEnumerator<BulkString> GetEnumerator() => list.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            public int Count => list.Count;
            public BulkString this[int index] => list[index];
            public override string ToString() => string.Join(", ", list);
        }
    }
}