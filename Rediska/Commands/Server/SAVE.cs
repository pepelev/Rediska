namespace Rediska.Commands.Server
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class SAVE : Command<None>
    {
        private static readonly BulkString[] request = {new PlainBulkString("SAVE")};
        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => request;
        public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
    }
}