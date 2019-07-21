using System.Collections.Generic;

namespace Rediska.Tests.Protocol.Responses
{
    public sealed class PlainArray : Array
    {
        private readonly IReadOnlyList<DataType> items;

        public PlainArray(IReadOnlyList<DataType> items)
        {
            this.items = items;
        }

        public override bool IsNull => false;
        public override IEnumerator<DataType> GetEnumerator() => items.GetEnumerator();
        public override int Count => items.Count;
        public override DataType this[int index] => items[index];
    }
}