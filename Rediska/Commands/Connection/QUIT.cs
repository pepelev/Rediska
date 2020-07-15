namespace Rediska.Commands.Connection
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class QUIT : Command<None>
    {
        private static readonly PlainBulkString name = new PlainBulkString("QUIT");
        private static readonly PlainBulkString[] request = {name};
        public static QUIT Singleton { get; } = new QUIT();
        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => request;
        public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
    }
}