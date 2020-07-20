namespace Rediska.Commands.Streams
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Protocol;
    using Protocol.Visitors;
    using Utils;
    using Array = Protocol.Array;

    public static partial class XINFO
    {
        public sealed class Consumer : IReadOnlyDictionary<string, DataType>
        {
            private readonly IReadOnlyDictionary<string, DataType> index;

            public Consumer(Array reply)
            {
                index = new PairsList<DataType>(reply).ToDictionary(
                    pair => pair.Left.Accept(BulkStringExpectation.Singleton).ToString(),
                    pair => pair.Right
                );
            }

            public ConsumerName Name => index["name"].Accept(BulkStringExpectation.Singleton).ToString();
            public long PendingEntries => index["pending"].Accept(IntegerExpectation.Singleton);

            public TimeSpan IdleTime => new TimeSpan(
                index["idle"].Accept(IntegerExpectation.Singleton) * TimeSpan.TicksPerMillisecond
            );

            IEnumerator<KeyValuePair<string, DataType>> IEnumerable<KeyValuePair<string, DataType>>.GetEnumerator() => index.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => index.GetEnumerator();
            int IReadOnlyCollection<KeyValuePair<string, DataType>>.Count => index.Count;
            bool IReadOnlyDictionary<string, DataType>.ContainsKey(string key) => index.ContainsKey(key);
            bool IReadOnlyDictionary<string, DataType>.TryGetValue(string key, out DataType value) => index.TryGetValue(key, out value);
            DataType IReadOnlyDictionary<string, DataType>.this[string key] => index[key];
            IEnumerable<string> IReadOnlyDictionary<string, DataType>.Keys => index.Keys;
            IEnumerable<DataType> IReadOnlyDictionary<string, DataType>.Values => index.Values;
        }
    }
}