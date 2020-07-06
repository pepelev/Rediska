namespace Rediska.Commands.Transactions
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class EXEC : Command<Array>
    {
        private static readonly BulkString[] request = {new PlainBulkString("EXEC")};
        public static EXEC Singleton { get; } = new EXEC();
        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => request;
        public override Visitor<Array> ResponseStructure => ArrayExpectation2.Singleton;
    }
}