using System.Collections.Generic;
using Rediska.Protocol;
using Rediska.Protocol.Visitors;
using Rediska.Utils;

namespace Rediska.Commands.Sets
{
    public sealed class SUNION : Command<IReadOnlyList<BulkString>>
    {
        private static readonly PlainBulkString name = new PlainBulkString("SUNION");

        private static readonly ListVisitor<BulkString> responseStructure = new ListVisitor<BulkString>(
            ArrayExpectation.Singleton,
            BulkStringExpectation.Singleton
        );

        private readonly IReadOnlyList<Key> keys;

        public SUNION(IReadOnlyList<Key> keys)
        {
            this.keys = keys;
        }

        public override DataType Request => new PlainArray(
            new PrefixedList<DataType>(
                name,
                new KeyList(keys)
            )
        );

        public override Visitor<IReadOnlyList<BulkString>> ResponseStructure => responseStructure;
    }
}