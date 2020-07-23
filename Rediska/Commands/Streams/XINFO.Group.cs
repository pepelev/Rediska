namespace Rediska.Commands.Streams
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public static partial class XINFO
    {
        public sealed class Group : IReadOnlyDictionary<string, DataType>
        {
            private readonly IReadOnlyDictionary<string, DataType> index;

            public Group(Array reply)
            {
                index = new PairsList<DataType>(reply).ToDictionary(
                    pair => pair.Left.Accept(BulkStringExpectation.Singleton).ToString(),
                    pair => pair.Right
                );
            }

            public GroupName Name => index["name"].Accept(BulkStringExpectation.Singleton).ToString();
            public long Consumers => index["consumers"].Accept(IntegerExpectation.Singleton);
            public long PendingEntries => index["pending"].Accept(IntegerExpectation.Singleton);
            public Id LastDelivered => index["last-delivered-id"].Accept(CompositeVisitors.StreamEntryId);

            IEnumerator<KeyValuePair<string, DataType>> IEnumerable<KeyValuePair<string, DataType>>.GetEnumerator() =>
                index.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => index.GetEnumerator();
            int IReadOnlyCollection<KeyValuePair<string, DataType>>.Count => index.Count;
            bool IReadOnlyDictionary<string, DataType>.ContainsKey(string key) => index.ContainsKey(key);
            bool IReadOnlyDictionary<string, DataType>.TryGetValue(string key, out DataType value) => index.TryGetValue(key, out value);
            public DataType this[string key] => index[key];
            IEnumerable<string> IReadOnlyDictionary<string, DataType>.Keys => index.Keys;
            IEnumerable<DataType> IReadOnlyDictionary<string, DataType>.Values => index.Values;
            public override string ToString() => Name;
        }
    }
}