﻿namespace Rediska.Commands.Keys
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class UNLINK : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("UNLINK");
        private readonly IReadOnlyList<Key> keys;

        public UNLINK(params Key[] keys)
            : this(keys as IReadOnlyList<Key>)
        {
            this.keys = keys;
        }

        public UNLINK(IReadOnlyList<Key> keys)
        {
            if (keys.Count == 0)
                throw new ArgumentException("Must contain elements", nameof(keys));

            this.keys = keys;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new PrefixedList<BulkString>(
            name,
            new KeyList(factory, keys)
        );

        public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;
    }
}