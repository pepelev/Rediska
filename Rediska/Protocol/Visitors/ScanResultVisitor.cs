using Rediska.Commands;

namespace Rediska.Protocol.Visitors
{
    using System;
    using System.Collections.Generic;
    using Commands.Hashes;
    using Array = Array;

    public static class ScanResultVisitor
    {
        public static ScanResultVisitor<BulkString> BulkStringList { get; } = new ScanResultVisitor<BulkString>(
            CompositeVisitors.BulkStringList
        );

        public static ScanResultVisitor<HashEntry> HashEntryList { get; } = new ScanResultVisitor<HashEntry>(
            CompositeVisitors.HashEntryList
        );
    }

    public sealed class ScanResultVisitor<T> : Visitor<ScanResult<T>>
    {
        private readonly Visitor<IReadOnlyList<T>> contentStructure;

        public ScanResultVisitor(Visitor<IReadOnlyList<T>> contentStructure)
        {
            this.contentStructure = contentStructure;
        }

        public override ScanResult<T> Visit(Integer integer)
            => throw new VisitException("Expected scan response", integer);

        public override ScanResult<T> Visit(SimpleString simpleString)
            => throw new VisitException("Expected scan response", simpleString);

        public override ScanResult<T> Visit(Error error)
            => throw new VisitException("Expected scan response", error);

        public override ScanResult<T> Visit(Array array)
        {
            if (array.Count != 2)
                throw new VisitException("Expected array with 2 elements", array);

            return new ScanResult<T>(
                new Cursor(
                    array[0].Accept(IntegerExpectation.Singleton)
                ),
                array[1].Accept(contentStructure)
            );
        }

        public override ScanResult<T> Visit(BulkString bulkString)
            => throw new VisitException("Expected scan response", bulkString);
    }
}