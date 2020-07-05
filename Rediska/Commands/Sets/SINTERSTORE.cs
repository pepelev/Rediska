namespace Rediska.Commands.Sets
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class SINTERSTORE : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("SINTERSTORE");
        private readonly Key destination;
        private readonly IReadOnlyList<Key> sources;

        public SINTERSTORE(Key destination, IReadOnlyList<Key> sources)
        {
            this.destination = destination;
            this.sources = sources;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new ConcatList<BulkString>(
            new[]
            {
                name,
                destination.ToBulkString(factory)
            },
            new KeyList(factory, sources)
        );

        public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;
    }
}