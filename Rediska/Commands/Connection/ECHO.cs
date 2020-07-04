namespace Rediska.Commands.Connection
{
    using Protocol;
    using Protocol.Visitors;

    public sealed class ECHO : Command<string>
    {
        private static readonly PlainBulkString name = new PlainBulkString("ECHO");

        private static readonly Visitor<string> responseStructure = BulkStringExpectation.Singleton
            .Then(bulk => bulk.ToString());

        private readonly string message;

        public ECHO(string message)
        {
            this.message = message;
        }

        public override DataType Request => new PlainArray(
            name,
            new PlainBulkString(message)
        );

        public override Visitor<string> ResponseStructure => responseStructure;
    }
}