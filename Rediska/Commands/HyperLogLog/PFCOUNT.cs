namespace Rediska.Commands.HyperLogLog
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class PFCOUNT : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("PFCOUNT");
        private readonly IReadOnlyList<Key> keys;

        public PFCOUNT(params Key[] keys)
            : this(keys as IReadOnlyList<Key>)
        {
        }

        public PFCOUNT(IReadOnlyList<Key> keys)
        {
            this.keys = keys;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new PrefixedList<BulkString>(
            name,
            new KeyList(keys)
        );

        public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;
    }
}