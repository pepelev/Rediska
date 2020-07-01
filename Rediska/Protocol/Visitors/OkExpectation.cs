namespace Rediska.Protocol.Visitors
{
    using Utils;

    public sealed class OkExpectation : Expectation<None>
    {
        public static OkExpectation Singleton { get; } = new OkExpectation();
        public override string Message => "OK";

        public override None Visit(SimpleString simpleString)
        {
            if (simpleString.Content == "OK")
                return new None();

            throw Exception(simpleString);
        }
    }
}