namespace Rediska.Commands.Transactions
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class MULTI : Command<None>
    {
        private static readonly BulkString[] request = {new PlainBulkString("MULTI")};
        public static MULTI Singleton { get; } = new MULTI();
        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => request;
        public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
    }
}