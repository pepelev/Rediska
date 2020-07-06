namespace Rediska.Commands.Server
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class BGREWRITEAOF : Command<string>
    {
        private static readonly BulkString[] request = {new PlainBulkString("BGREWRITEAOF")};
        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => request;
        public override Visitor<string> ResponseStructure => SimpleStringExpectation.Singleton;
    }
}