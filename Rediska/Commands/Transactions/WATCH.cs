namespace Rediska.Commands.Transactions
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class WATCH : Command<None>
    {
        private static readonly PlainBulkString name = new PlainBulkString("WATCH");
        private readonly IReadOnlyList<Key> keys;

        public WATCH(params Key[] keys)
            : this(keys as IReadOnlyList<Key>)
        {
        }

        public WATCH(IReadOnlyList<Key> keys)
        {
            this.keys = keys;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new PrefixedList<BulkString>(
            name,
            new KeyList(keys)
        );

        public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
    }
}