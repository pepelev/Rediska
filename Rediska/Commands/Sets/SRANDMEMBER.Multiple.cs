namespace Rediska.Commands.Sets
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public static partial class SRANDMEMBER
    {
        public sealed class Multiple : Command<IReadOnlyList<BulkString>>
        {
            private static readonly ListVisitor<BulkString> responseStructure = new ListVisitor<BulkString>(
                ArrayExpectation.Singleton,
                BulkStringExpectation.Singleton
            );

            private readonly Key key;
            private readonly Count count;

            public Multiple(Key key, Count count)
            {
                this.key = key;
                this.count = count;
            }

            public override DataType Request => new PlainArray(
                name,
                key.ToBulkString(),
                count.ToBulkString()
            );

            public override Visitor<IReadOnlyList<BulkString>> ResponseStructure => responseStructure;
        }
    }
}