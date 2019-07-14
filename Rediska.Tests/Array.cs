using System.Collections.Generic;

namespace Rediska.Tests
{
    public sealed class Array : DataType
    {
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
    }
}