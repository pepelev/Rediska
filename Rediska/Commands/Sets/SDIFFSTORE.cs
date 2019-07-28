using System.Collections.Generic;
using Rediska.Protocol;
using Rediska.Protocol.Visitors;
using Rediska.Utils;

namespace Rediska.Commands.Sets
{
    public sealed class SDIFFSTORE : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("SDIFFSTORE");

        private readonly Key destination;
        private readonly IReadOnlyList<Key> sources;

        public SDIFFSTORE(Key destination, params Key[] sources)
            : this(destination, sources as IReadOnlyList<Key>)
        {
        }

        public SDIFFSTORE(Key destination, IReadOnlyList<Key> sources)
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