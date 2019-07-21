namespace Rediska.Tests.Protocol.Responses.Visitors
{
    public sealed class ConstVisitor<T> : Visitor<T>
    {
        private readonly T value;

        public ConstVisitor(T value)
        {
            this.value = value;
        }

        public override T Visit(Integer integer) => value;
        public override T Visit(SimpleString simpleString) => value;
        public override T Visit(Error error) => value;
        public override T Visit(Array array) => value;
        public override T Visit(BulkString bulkString) => value;
    }
}