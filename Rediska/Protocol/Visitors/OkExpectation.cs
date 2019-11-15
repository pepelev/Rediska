namespace Rediska.Protocol.Visitors
{
    using Utils;

    public sealed class OkExpectation : Visitor<None>
    {
        public static OkExpectation Singleton { get; } = new OkExpectation();

        public override None Visit(Integer integer)
        {
            throw new VisitException("OK expected", integer);
        }

        public override None Visit(SimpleString simpleString)
        {
            if (simpleString.Content == "OK")
                return new None();

            throw new VisitException("OK expected", simpleString);
        }

        public override None Visit(Error error)
        {
            throw new VisitException("OK expected", error);
        }

        public override None Visit(Array array)
        {
            throw new VisitException("OK expected", array);
        }

        public override None Visit(BulkString bulkString)
        {
            throw new VisitException("OK expected", bulkString);
        }
    }
}