namespace Rediska.Commands.Strings
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class MGET : Command<IReadOnlyList<BulkString>>
    {
        private static readonly PlainBulkString name = new PlainBulkString("MGET");
        private readonly IReadOnlyList<Key> keys;

        public MGET(params Key[] keys)
            : this(keys as IReadOnlyList<Key>)
        {
        }

        public MGET(IReadOnlyList<Key> keys)
        {
            if (keys.Count < 1)
                throw new ArgumentException("Must contain at least one key", nameof(keys));

            this.keys = keys;
        }

        public override DataType Request => new PlainArray(
            new PrefixedList<DataType>(
                name,
                new KeyList(keys)
            )
        );

        public override Visitor<IReadOnlyList<BulkString>> ResponseStructure => CompositeVisitors.BulkStringList;
    }
}