using System;
using System.Collections;
using System.Collections.Generic;
using Rediska.Tests.Protocol.Responses.Visitors;

namespace Rediska.Tests.Protocol.Responses
{
    public abstract class Array : DataType, IReadOnlyList<DataType>
    {
        public static Array Null { get; } = new NullArray();

        public abstract bool IsNull { get; }
        public abstract IEnumerator<DataType> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public abstract int Count { get; }
        public abstract DataType this[int index] { get; }
        public override T Accept<T>(Visitor<T> visitor) => visitor.Visit(this);

        private sealed class NullArray : Array
        {
            public override bool IsNull => true;
            public override IEnumerator<DataType> GetEnumerator() => Enumerator.Singleton;
            public override int Count => 0;
            public override DataType this[int index] => throw new ArgumentOutOfRangeException(nameof(index));

            private sealed class Enumerator : IEnumerator<DataType>
            {
                public static Enumerator Singleton { get; }= new Enumerator();

                public void Dispose()
                {
                }

                public bool MoveNext() => false;

                public void Reset()
                {
                }

                public DataType Current => throw new InvalidOperationException();
                object IEnumerator.Current => Current;
            }
        }
    }
}