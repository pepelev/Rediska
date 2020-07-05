namespace Rediska.Commands.Sets
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public static partial class SRANDMEMBER
    {
        public sealed class Multiple : Command<IReadOnlyList<BulkString>>
        {
            private readonly Key key;
            private readonly Count count;

            public Multiple(Key key, Count count)
            {
                this.key = key;
                this.count = count;
            }

            public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
            {
                name,
                key.ToBulkString(factory),
                count.ToBulkString(factory)
            };

            public override Visitor<IReadOnlyList<BulkString>> ResponseStructure => CompositeVisitors.BulkStringList;
        }
    }
}