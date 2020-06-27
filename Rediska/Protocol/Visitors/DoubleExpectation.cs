namespace Rediska.Protocol.Visitors
{
    public sealed class DoubleExpectation : Visitor<double>
    {
        public static DoubleExpectation Singleton { get; } = new DoubleExpectation();

        public override double Visit(Integer integer) => throw new VisitException("Double expected", integer);
        public override double Visit(SimpleString simpleString) => throw new VisitException("Double expected", simpleString);
        public override double Visit(Error error) => throw new VisitException("Double expected", error);
        public override double Visit(Array array) => throw new VisitException("Double expected", array);
        public override double Visit(BulkString bulkString) => bulkString.ToDouble();
    }
}