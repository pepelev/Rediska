namespace Rediska.Protocol.Visitors
{
    public sealed class SimpleStringExpectation : Visitor<string>
    {
        public static SimpleStringExpectation Singleton { get; } = new SimpleStringExpectation();

        public override string Visit(Integer integer)
        {
            throw new VisitException("Simple string expected", integer);
        }

        public override string Visit(SimpleString simpleString) => simpleString.Content;

        public override string Visit(Error error)
        {
            throw new VisitException("Simple string expected", error);
        }

        public override string Visit(Array array)
        {
            throw new VisitException("Simple string expected", array);
        }

        public override string Visit(BulkString bulkString)
        {
            throw new VisitException("Simple string expected", bulkString);
        }
    }
}