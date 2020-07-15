namespace Rediska.Commands.Connection
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class PING : Command<string>
    {
        private const string DefaultMessage = "PONG";
        private static readonly PlainBulkString name = new PlainBulkString("PING");
        private static readonly PlainBulkString[] simpleRequest = {name};
        private readonly string message;

        public PING()
            : this(DefaultMessage)
        {
        }

        public PING(string message)
        {
            this.message = message;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => message == DefaultMessage
            ? simpleRequest
            : new[] {name, factory.Utf8(message)};

        public override Visitor<string> ResponseStructure => SimpleStringExpectation.Singleton;
    }
}