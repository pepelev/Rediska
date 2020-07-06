namespace Rediska.Commands.Connection
{
    using System.Collections.Generic;
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

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            factory.Utf8(message)
        };

        public override Visitor<string> ResponseStructure => responseStructure;
    }
}