using System.Collections.Generic;
using Rediska.Protocol.Outputs;

namespace Rediska.Protocol
{
    public sealed class PlainArray : Array
    {
        private readonly IReadOnlyList<DataType> items;

        public PlainArray(params DataType[] items)
            : this(items as IReadOnlyList<DataType>)
        {
        }

        public PlainArray(IReadOnlyList<DataType> items)
        {
            this.items = items;
        }

        public override bool IsNull => false;
        public override int Count => items.Count;
        public override DataType this[int index] => items[index];
        public override IEnumerator<DataType> GetEnumerator() => items.GetEnumerator();

        public override void Write(Output output)
        {
            output.Write(Magic.Array);
            output.Write(items.Count);
            output.WriteCRLF();
            foreach (var element in items)
                element.Write(output);
        }
    }
}