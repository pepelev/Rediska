namespace Rediska.Protocol.Visitors
{
    public abstract class Expectation<T> : Visitor<T>
    {
        public abstract string Message { get; }
        protected VisitException Exception(DataType subject) => new VisitException($"Expected {Message}", subject);
        public override T Visit(Integer integer) => throw Exception(integer);
        public override T Visit(SimpleString simpleString) => throw Exception(simpleString);
        public override T Visit(Error error) => throw Exception(error);
        public override T Visit(Array array) => throw Exception(array);
        public override T Visit(BulkString bulkString) => throw Exception(bulkString);
        public override string ToString() => $"Visitor that expects {Message}";
    }
}