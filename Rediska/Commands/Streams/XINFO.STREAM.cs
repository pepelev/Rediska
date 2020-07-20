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
        public sealed class Stream : IReadOnlyDictionary<string, DataType>
        {
            private readonly IReadOnlyDictionary<string, DataType> index;

            public Stream(Array reply)
            {
                index = new PairsList<DataType>(reply).ToDictionary(
                    pair => pair.Left.Accept(BulkStringExpectation.Singleton).ToString(),
                    pair => pair.Right
                );
            }

            public long Length => index["length"].Accept(IntegerExpectation.Singleton);
            public long RadixTreeKeys => index["radix-tree-keys"].Accept(IntegerExpectation.Singleton);
            public long RadixTreeNodes => index["radix-tree-nodes"].Accept(IntegerExpectation.Singleton);
            public long Groups => index["groups"].Accept(IntegerExpectation.Singleton);

            public Id LastGeneratedId => Id.Parse(
                index["last-generated-id"].Accept(BulkStringExpectation.Singleton).ToString()
            );

            public Entry FirstEntry => new Entry(
                index["first-entry"].Accept(ArrayExpectation2.Singleton)
            );

            public Entry LastEntry => new Entry(
                index["last-entry"].Accept(ArrayExpectation2.Singleton)
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

        public sealed class STREAM : Command<Stream>
        {
            private static readonly PlainBulkString subName = new PlainBulkString("STREAM");

            private static readonly Visitor<Stream> responseStructure = ArrayExpectation2.Singleton
                .Then(reply => new Stream(reply));

            private readonly Key key;

            public STREAM(Key key)
            {
                this.key = key ?? throw new ArgumentNullException(nameof(key));
            }

            public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
            {
                name,
                subName,
                key.ToBulkString(factory)
            };

            public override Visitor<Stream> ResponseStructure => responseStructure;
        }
    }
}