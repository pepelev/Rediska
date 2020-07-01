namespace Rediska.Commands.Server
{
    using Protocol;
    using Protocol.Visitors;

    public sealed class BGREWRITEAOF : Command<string>
    {
        private static readonly PlainArray request = new PlainArray(
            new PlainBulkString("BGREWRITEAOF")
        );

        public override DataType Request => request;
        public override Visitor<string> ResponseStructure => SimpleStringExpectation.Singleton;
    }
}