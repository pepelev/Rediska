namespace Rediska.Commands.Sets
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public static partial class SPOP
    {
        public sealed class Multiple : Command<IReadOnlyList<BulkString>>
        {
            private readonly long count;
            private readonly Key key;

            public Multiple(Key key, long count)
            {
                if (count < 1)
                    throw new ArgumentOutOfRangeException(nameof(count), count, "Must be positive");

                this.key = key;
                this.count = count;
            }

            public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
            {
                name,
                key.ToBulkString(factory),
                factory.Create(count)
            };

            public override Visitor<IReadOnlyList<BulkString>> ResponseStructure => CompositeVisitors.BulkStringList;
        }
    }
}