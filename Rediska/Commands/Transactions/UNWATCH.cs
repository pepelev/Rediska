namespace Rediska.Commands.Transactions
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class UNWATCH : Command<None>
    {
        private static readonly BulkString[] request = {new PlainBulkString("UNWATCH")};
        public static UNWATCH Singleton { get; } = new UNWATCH();
        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => request;
        public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
    }
}