namespace Rediska.Protocol.Visitors
{
    public sealed class IntegerExpectation : Visitor<long>
    {
        public static IntegerExpectation Singleton { get; } = new IntegerExpectation();

        public override long Visit(Integer integer) => integer.Value;
        public override long Visit(SimpleString simpleString) => throw new VisitException("Integer expected", simpleString);
        public override long Visit(Error error) => throw new VisitException("Integer expected", error);
        public override long Visit(Array array) => throw new VisitException("Integer expected", array);
        public override long Visit(BulkString bulkString) => throw new VisitException("Integer expected", bulkString);
    }
}