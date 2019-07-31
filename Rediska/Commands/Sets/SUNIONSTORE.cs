namespace Rediska.Commands.Sets
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class SUNIONSTORE : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("SUNIONSTORE");
        private readonly Key destination;
        private readonly IReadOnlyList<Key> sources;

        public SUNIONSTORE(Key destination, IReadOnlyList<Key> sources)
        {
            this.destination = destination;
            this.sources = sources;
        }

        public override DataType Request => new PlainArray(
            new ConcatList<DataType>(
                new DataType[]
                {
                    name,
                    destination.ToBulkString()
                },
                new KeyList(sources)
            )
        );

        public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;
    }
}