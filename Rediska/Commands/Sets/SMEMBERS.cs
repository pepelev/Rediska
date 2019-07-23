using System.Collections.Generic;
using Rediska.Protocol;
using Rediska.Protocol.Visitors;

namespace Rediska.Commands.Sets
{
    public sealed class SMEMBERS : Command<IReadOnlyList<BulkString>>
    {
        private static readonly PlainBulkString name = new PlainBulkString("SMEMBERS");

        private static readonly ListVisitor<BulkString> responseStructure = new ListVisitor<BulkString>(
            ArrayExpectation.Singleton,
            BulkStringExpectation.Singleton
        );

        private readonly Key key;

        public SMEMBERS(Key key)
        {
            this.key = key;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString()
        );

        public override Visitor<IReadOnlyList<BulkString>> ResponseStructure => responseStructure;
    }
}