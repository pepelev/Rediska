namespace Rediska.Protocol.Visitors
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using Commands.Keys;
    using Commands.Lists;

    public static class CompositeVisitors
    {
        public static Visitor<ExpireResult> ExpireResult { get; } =
            IntegerExpectation.Singleton.Then(ParseExpireResult);

        public static Visitor<IReadOnlyList<BulkString>> BulkStringList { get; } = new ListVisitor<BulkString>(
            ArrayExpectation.Singleton,
            BulkStringExpectation.Singleton
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

        private static ExpireResult ParseExpireResult(long integer)
        {
            switch (integer)
            {
                case 0:
                    return Commands.Keys.ExpireResult.KeyNotExists;
                case 1:
                    return Commands.Keys.ExpireResult.TimeoutSet;
                default:
                    throw new Exception($"Expected 0 or 1, but '{integer}' received");
            }
        }

        public static Visitor<IReadOnlyList<Commands.Hashes.HashEntry>> HashEntryList { get; } =
            ArrayExpectation.Singleton.Then(list => new Response(list) as IReadOnlyList<Commands.Hashes.HashEntry>);

        private sealed class Response : IReadOnlyList<HashEntry>
        {
            private readonly IReadOnlyList<DataType> list;

            public Response(IReadOnlyList<DataType> list)
            {
                this.list = list;
            }

            public IEnumerator<HashEntry> GetEnumerator()
            {
                for (var i = 0; i < Count; i++)
                    yield return this[i];
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public int Count => list.Count / 2;

            public HashEntry this[int index] => 0 <= index && index < Count
                ? new HashEntry(index, list)
                : throw new IndexOutOfRangeException();
        }
        private sealed class HashEntry : Commands.Hashes.HashEntry
        {
            private readonly int index;

            public HashEntry(int index, IReadOnlyList<DataType> list)
            {
                this.index = index;
                this.list = list;
            }

            private readonly IReadOnlyList<DataType> list;

            public override Key Key => new Key.BulkString(
                list[index * 2].Accept(BulkStringExpectation.Singleton)
            );

            public override BulkString Value => list[index * 2 + 1].Accept(
                BulkStringExpectation.Singleton
            );
        }
    }
}