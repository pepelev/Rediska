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

            private static readonly Visitor<IReadOnlyList<(string Name, string Value)>> responseStructure = ArrayExpectation.Singleton
                .Then(
                    pairs => new ProjectingReadOnlyList<(DataType Name, DataType Value), (string Name, string Value)>(
                        new PairsList<DataType>(pairs),
                        pair => (String(pair.Name), String(pair.Value))
                    ) as IReadOnlyList<(string Name, string Value)>
                );

            private readonly string pattern;

            public GET(string pattern)
            {
                this.pattern = pattern;
            }

            public override DataType Request => new PlainArray(
                name,
                subName,
                new PlainBulkString(pattern)
            );

            public override Visitor<IReadOnlyList<(string Name, string Value)>> ResponseStructure => responseStructure;

            private static string String(DataType dateTime) =>
                dateTime.Accept(BulkStringExpectation.Singleton).ToString();
        }
    }
}