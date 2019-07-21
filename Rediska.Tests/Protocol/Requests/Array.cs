using System.Collections.Generic;
using Rediska.Tests.Checks;

namespace Rediska.Tests.Protocol.Requests
{
    public sealed class Array : DataType
    {
        public static DataType Null { get; } = new NullArray();

        private readonly IReadOnlyCollection<DataType> elements;

        public Array(params DataType[] elements)
        {
            this.elements = elements;
        }

        public Array(IReadOnlyCollection<DataType> elements)
        {
            this.elements = elements;
        }

        public override void Write(Output output)
        {
            output.Write(Magic.Array);
            output.Write(elements.Count);
            output.WriteCRLF();
            foreach (var element in elements)
                element.Write(output);
        }

        public override string ToString() => string.Join(" ", elements);

        private sealed class NullArray : DataType
        {
            public override void Write(Output output)
            {
                output.Write(Magic.Array);
                output.Write(-1);
                output.WriteCRLF();
            }
        }
    }
}