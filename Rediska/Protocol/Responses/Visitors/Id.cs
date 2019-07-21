namespace Rediska.Protocol.Responses.Visitors
{
    public sealed class Id : Visitor<DataType>
    {
        public static Id Singleton { get; } = new Id();

        public override DataType Visit(Integer integer) => integer;
        public override DataType Visit(SimpleString simpleString) => simpleString;
        public override DataType Visit(Error error) => error;
        public override DataType Visit(Array array) => array;
        public override DataType Visit(BulkString bulkString) => bulkString;
    }
}