namespace Rediska.Protocol.Visitors
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using Commands.Keys;
    using Commands.Lists;
    using Utils;

    public static class CompositeVisitors
    {
        public static Visitor<ExpireResponse> ExpireResult { get; } =
            IntegerExpectation.Singleton.Then(ParseExpireResult);

        public static Visitor<IReadOnlyList<BulkString>> BulkStringList { get; } = new ListVisitor<BulkString>(
            ArrayExpectation.Singleton,
            BulkStringExpectation.Singleton
        );

        public static Visitor<IReadOnlyList<long>> IntegerList { get; } = new ListVisitor<long>(
            ArrayExpectation.Singleton,
            IntegerExpectation.Singleton
        );

        public static Visitor<IReadOnlyList<string>> SimpleStringList { get; } = new ListVisitor<string>(
            ArrayExpectation.Singleton,
            SimpleStringExpectation.Singleton
        );

        public static Visitor<IReadOnlyList<Key>> KeyList { get; } = new ListVisitor<Key>(
            ArrayExpectation.Singleton,
            BulkStringExpectation.Singleton.Then(@string => new Key.BulkString(@string) as Key)
        );

        public static Visitor<PopResult> Pop { get; } = ArrayExpectation2.Singleton.Then(array => new PopResult(array));

        public static Visitor<double> Double = BulkStringExpectation.Singleton
            .Then(
                @string => double.Parse(
                    Encoding.UTF8.GetString(
                        @string.ToBytes()
                    ),
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture
                )
            );

        private static ExpireResponse ParseExpireResult(long integer)
        {
            switch (integer)
            {
                case 0:
                    return Commands.Keys.ExpireResponse.KeyNotExists;
                case 1:
                    return Commands.Keys.ExpireResponse.TimeoutSet;
                default:
                    throw new Exception($"Expected 0 or 1, but '{integer}' received");
            }
        }

        public static Visitor<IReadOnlyList<(BulkString Field, BulkString Value)>> HashEntryList { get; } =
            BulkStringList.Then(
                list =>
                {
                    var pairs = new PairsList<BulkString>(list);
                    return (IReadOnlyList<(BulkString Field, BulkString Value)>) pairs;
                }
            );

        public static Visitor<IReadOnlyList<(BulkString Member, double Score)>> SortedSetEntryList { get; } =
            BulkStringList.Then(
                list => new ProjectingReadOnlyList<
                    (BulkString Member, BulkString Score),
                    (BulkString Member, double Score)>(
                    new PairsList<BulkString>(list),
                    pair => (pair.Member, pair.Score.ToDouble())
                ) as IReadOnlyList<(BulkString Member, double Score)>
            );
    }
}