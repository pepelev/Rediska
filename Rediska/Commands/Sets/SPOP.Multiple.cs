using System.Globalization;
using Array = Rediska.Tests.Protocol.Requests.Array;
using DataType = Rediska.Tests.Protocol.Requests.DataType;

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

            public override DataType Request => new Array(
                name,
                key.ToBulkString(),
                new Protocol.Requests.BulkString(count.ToString(CultureInfo.InvariantCulture))
            );

            public override Visitor<IReadOnlyList<BulkString>> ResponseStructure => responseStructure;
        }
    }
}