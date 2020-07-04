namespace Rediska.Commands.HyperLogLog
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class PFMERGE : Command<None>
    {
        private static readonly PlainBulkString name = new PlainBulkString("PFMERGE");
        private readonly Key destination;
        private readonly IReadOnlyList<Key> sources;

        public PFMERGE(Key destination, params Key[] sources)
            : this(destination, sources as IReadOnlyList<Key>)
        {
        }

        public PFMERGE(Key destination, IReadOnlyList<Key> sources)
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

        public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
    }
}