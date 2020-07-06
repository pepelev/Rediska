namespace Rediska.Commands.Sets
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class SDIFF : Command<IReadOnlyList<BulkString>>
    {
        private static readonly PlainBulkString name = new PlainBulkString("SDIFF");
        private readonly Key minuend;
        private readonly IReadOnlyList<Key> subtrahends;

        public SDIFF(Key minuend, params Key[] subtrahends)
            : this(minuend, subtrahends as IReadOnlyList<Key>)
        {
        }

        public SDIFF(Key minuend, IReadOnlyList<Key> subtrahends)
        {
            this.minuend = minuend;
            this.subtrahends = subtrahends;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new PrefixedList<BulkString>(
            name,
            new KeyList(
                factory,
                new PrefixedList<Key>(
                    minuend,
                    subtrahends
                )
            )
        );

        public override Visitor<IReadOnlyList<BulkString>> ResponseStructure => CompositeVisitors.BulkStringList;
    }
}