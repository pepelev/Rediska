namespace Rediska.Commands.Keys
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed partial class SORT : Command<IReadOnlyList<SORT.Item>>
    {
        private static readonly PlainBulkString name = new PlainBulkString("SORT");
        private static readonly PlainBulkString limitSegment = new PlainBulkString("LIMIT");
        private static readonly PlainBulkString descendingSegment = new PlainBulkString("DESC");
        private static readonly PlainBulkString alphabeticalSegment = new PlainBulkString("ALPHA");
        private readonly Key key;
        private readonly By by;
        private readonly Limit? limit;
        private readonly Select select;
        private readonly Order order;
        private readonly Mode mode;

        public SORT(Key key, By by, Limit? limit, Select select, Order order, Mode mode)
        {
            this.key = key;
            this.by = by;
            this.limit = limit;
            this.select = select;
            this.order = order;
            this.mode = mode;
        }

        public override DataType Request => new PlainArray(
            Query().ToList()
        );

        public override Visitor<IReadOnlyList<Item>> ResponseStructure => CompositeVisitors.BulkStringList
            .Then(
                response =>
                {
                    var bulkStringsPerItem = select.BulkStringsPerItem;
                    var responseSize = response.Count;
                    if (responseSize % bulkStringsPerItem != 0)
                    {
                        throw new ArgumentException(
                            "Size must be multiple of BulkStringPerItem",
                            nameof(responseSize)
                        );
                    }

                    var result = new Item[responseSize / bulkStringsPerItem];
                    for (var i = 0; i < result.Length; i++)
                    {
                        result[i] = new Item(
                            new SubList<BulkString>(
                                response,
                                i * bulkStringsPerItem,
                                bulkStringsPerItem
                            )
                        );
                    }

                    return (IReadOnlyList<Item>) result;
                }
            );

        private IEnumerable<BulkString> Query()
        {
            yield return name;
            yield return key.ToBulkString();
            foreach (var segment in by.Query())
            {
                yield return segment;
            }

            if (limit is {} value)
            {
                yield return limitSegment;
                yield return value.Offset.ToBulkString();
                yield return value.Count.ToBulkString();
            }

            foreach (var segment in select.Query())
            {
                yield return segment;
            }

            if (order == Order.Descending)
                yield return descendingSegment;

            if (mode == Mode.Alphabetical)
                yield return alphabeticalSegment;
        }
    }
}