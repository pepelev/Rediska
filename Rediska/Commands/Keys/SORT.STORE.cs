namespace Rediska.Commands.Keys
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Protocol;
    using Protocol.Visitors;

    public sealed partial class SORT
    {
        public sealed class STORE : Command<STORE.Response>
        {
            private static readonly PlainBulkString store = new PlainBulkString("STORE");
            private readonly Key key;
            private readonly By by;
            private readonly Limit? limit;
            private readonly Select select;
            private readonly Order order;
            private readonly Mode mode;
            private readonly Key destination;

            public STORE(
                Key key,
                By by,
                Limit? limit,
                Select select,
                Order order,
                Mode mode,
                Key destination)
            {
                this.key = key ?? throw new ArgumentNullException(nameof(key));
                this.by = by ?? throw new ArgumentNullException(nameof(by));
                this.limit = limit;
                this.select = select ?? throw new ArgumentNullException(nameof(select));
                this.order = order; // todo validate order and mode and in regular SORT
                this.mode = mode;
                this.destination = destination ?? throw new ArgumentNullException(nameof(destination));
            }

            public override IEnumerable<BulkString> Request(BulkStringFactory factory)
            {
                yield return name;
                yield return key.ToBulkString(factory);
                foreach (var argument in by.Arguments(factory))
                {
                    yield return argument;
                }

                if (limit is { } value)
                {
                    yield return limitArgument;
                    yield return factory.Create(value.Offset);
                    yield return factory.Create(value.Count);
                }

                foreach (var argument in select.Arguments(factory))
                {
                    yield return argument;
                }

                if (order == Order.Descending)
                    yield return descendingArgument;

                if (mode == Mode.Alphabetical)
                    yield return alphabeticalArgument;

                yield return store;
                yield return destination.ToBulkString(factory);
            }

            public override Visitor<Response> ResponseStructure => IntegerExpectation.Singleton
                .Then(resultingListLength => new Response(select.BulkStringsPerItem, resultingListLength));

            public readonly struct Response
            {
                public Response(long bulkStringsPerItem, long resultingListLength)
                {
                    if (resultingListLength % bulkStringsPerItem != 0)
                    {
                        throw new ArgumentException(
                            $"{nameof(resultingListLength)} must be multiple of {nameof(bulkStringsPerItem)}" + 
                            $" but {resultingListLength} and {bulkStringsPerItem} found"
                        );
                    }

                    BulkStringsPerItem = bulkStringsPerItem;
                    StoredItems = resultingListLength / bulkStringsPerItem;
                }

                public long BulkStringsPerItem { get; }
                public long StoredItems { get; }
                public long ResultingListLength => BulkStringsPerItem * StoredItems;

                public override string ToString() =>
                    $"{nameof(BulkStringsPerItem)}: {BulkStringsPerItem.ToString(CultureInfo.InvariantCulture)}, " +
                    $"{nameof(StoredItems)}: {StoredItems.ToString(CultureInfo.InvariantCulture)}";
            }
        }
    }
}