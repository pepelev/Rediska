using System;
using System.Collections;
using System.Collections.Generic;
using Rediska.Protocol.Outputs;
using Rediska.Protocol.Visitors;
using Rediska.Utils;

namespace Rediska.Protocol
{
    public abstract class Array : DataType, IReadOnlyList<DataType>
    {
        public static Array Null { get; } = new NullArray();
        public static Array Empty { get; } = new PlainArray();
        public abstract bool IsNull { get; }
        public abstract IEnumerator<DataType> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public abstract int Count { get; }
        public abstract DataType this[int index] { get; }
        public override T Accept<T>(Visitor<T> visitor) => visitor.Visit(this);

        private sealed class NullArray : Array
        {
            public override bool IsNull => true;
            public override IEnumerator<DataType> GetEnumerator() => EmptyEnumerator<DataType>.Singleton;
            public override int Count => 0;
            public override DataType this[int index] => throw new ArgumentOutOfRangeException(nameof(index));

            public override void Write(Output output)
            {
                output.Write(Magic.Array);
                output.Write(-1);
                output.WriteCRLF();
            }
        }
    }
}