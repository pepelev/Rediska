using System;
using System.Collections;
using System.Collections.Generic;
using Rediska.Protocol;
using Rediska.Protocol.Visitors;

namespace Rediska.Commands.Hashes
{
    public sealed class HGETALL : Command<IReadOnlyList<HashEntry>>
    {
        private static readonly BulkString name = new PlainBulkString("HGETALL");

        private readonly Key key;

        public HGETALL(Key key)
        {
            this.key = key;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString()
        );

        public override Visitor<IReadOnlyList<Hashes.HashEntry>> ResponseStructure => ArrayExpectation.Singleton
            .Then(list => new Response(list) as IReadOnlyList<Hashes.HashEntry>);

        private sealed class Response : IReadOnlyList<HashEntry>
        {
            private readonly IReadOnlyList<Protocol.DataType> list;

            public Response(IReadOnlyList<Protocol.DataType> list)
            {
                this.list = list;
            }

            public IEnumerator<HashEntry> GetEnumerator()
            {
                for (var i = 0; i < Count; i++)
                    yield return this[i];
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public int Count => list.Count / 2;

            public HashEntry this[int index] => 0 <= index && index < Count 
                ? new HashEntry(index, list)
                : throw new IndexOutOfRangeException();
        }

        private sealed class HashEntry : Hashes.HashEntry
        {
            private readonly int index;

            public HashEntry(int index, IReadOnlyList<Protocol.DataType> list)
            {
                this.index = index;
                this.list = list;
            }

            private readonly IReadOnlyList<Protocol.DataType> list;

            public override Key Key => new Key.BulkString(
                list[index * 2].Accept(BulkStringExpectation.Singleton)
            );

            public override Protocol.BulkString Value => list[index * 2 + 1].Accept(
                BulkStringExpectation.Singleton
            );
        }
    }
}