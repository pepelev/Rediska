namespace Rediska.Commands.Sets
{
    using System.Collections.Generic;
    using System.Globalization;
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
                this.key = key;
                this.count = count;
            }

            public override DataType Request => new PlainArray(
                name,
                key.ToBulkString(),
                new PlainBulkString(count.ToString(CultureInfo.InvariantCulture))
            );

            public override Visitor<IReadOnlyList<BulkString>> ResponseStructure => CompositeVisitors.BulkStringList;
        }
    }
}