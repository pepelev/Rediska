namespace Rediska.Commands.Transactions
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class DISCARD : Command<None>
    {
        private static readonly BulkString[] request = {new PlainBulkString("DISCARD")};
        public static DISCARD Singleton { get; } = new DISCARD();
        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => request;
        public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
    }
}