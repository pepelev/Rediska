namespace Rediska.Commands.Server
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public static partial class CONFIG
    {
        public sealed class GET : Command<IReadOnlyList<(string Name, string Value)>>
        {
            private static readonly PlainBulkString subName = new PlainBulkString("GET");

            private static readonly Visitor<IReadOnlyList<(string Name, string Value)>> responseStructure = CompositeVisitors.BulkStringList
                .Then(
                    pairs => new ProjectingReadOnlyList<(BulkString Name, BulkString Value), (string Name, string Value)>(
                        new PairsList<BulkString>(pairs),
                        pair => (pair.Name.ToString(), pair.Value.ToString())
                    ) as IReadOnlyList<(string Name, string Value)>
                );

            private readonly string pattern;

            public GET(string pattern)
            {
                this.pattern = pattern;
            }

            public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
            {
                name,
                subName,
                factory.Utf8(pattern)
            };

            public override Visitor<IReadOnlyList<(string Name, string Value)>> ResponseStructure => responseStructure;
        }
    }
}