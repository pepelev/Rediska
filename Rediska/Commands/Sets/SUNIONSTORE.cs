using System.Collections.Generic;
using Rediska.Protocol;
using Rediska.Protocol.Visitors;
using Rediska.Utils;

namespace Rediska.Commands.Sets
{
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