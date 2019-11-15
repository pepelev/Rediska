using Rediska.Commands;

namespace Rediska.Protocol.Visitors
{
    public sealed class ScanResultVisitor : Visitor<ScanResult>
    {
        public static ScanResultVisitor Singleton { get; } = new ScanResultVisitor();

        public override ScanResult Visit(Integer integer)
            => throw new VisitException("Expected scan response", integer);

        public override ScanResult Visit(SimpleString simpleString)
            => throw new VisitException("Expected scan response", simpleString);

        public override ScanResult Visit(Error error)
            => throw new VisitException("Expected scan response", error);

        public override ScanResult Visit(Array array)
        {
            if (array.Count != 2)
                throw new VisitException("Expected array with 2 elements", array);

            return new ScanResult(
                new Cursor(
                    array[0].Accept(IntegerExpectation.Singleton)
                ),
                array[1].Accept(CompositeVisitors.BulkStringList)
            );
        }

        public override ScanResult Visit(BulkString bulkString)
            => throw new VisitException("Expected scan response", bulkString);
    }
}