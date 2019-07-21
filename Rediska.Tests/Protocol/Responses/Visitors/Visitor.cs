namespace Rediska.Tests.Protocol.Responses.Visitors
{
    public abstract class Visitor<T>
    {
        public abstract T Visit(Integer integer);
        public abstract T Visit(SimpleString simpleString);
        public abstract T Visit(Error error);
        public abstract T Visit(Array array);
        public abstract T Visit(BulkString bulkString);
    }
}