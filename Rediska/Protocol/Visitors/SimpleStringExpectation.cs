namespace Rediska.Protocol.Visitors
{
    public sealed class SimpleStringExpectation : Expectation<string>
    {
        public static SimpleStringExpectation Singleton { get; } = new SimpleStringExpectation();
        public override string Message => "Simple string";
        public override string Visit(SimpleString simpleString) => simpleString.Content;
    }
}