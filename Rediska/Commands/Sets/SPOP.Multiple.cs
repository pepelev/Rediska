using System.Collections.Generic;
using System.Globalization;
using Rediska.Protocol;
using Rediska.Protocol.Visitors;

namespace Rediska.Commands.Sets
{
    public static partial class SPOP
    {
        public sealed class Multiple : Command<IReadOnlyList<BulkString>>
        {
            private static readonly ListVisitor<BulkString> responseStructure = new ListVisitor<BulkString>(
                ArrayExpectation.Singleton,
                BulkStringExpectation.Singleton
            );

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

            public override Visitor<IReadOnlyList<BulkString>> ResponseStructure => responseStructure;
        }
    }
}