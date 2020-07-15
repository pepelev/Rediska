namespace Rediska.Commands.Connection
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public static partial class CLIENT
    {
        public sealed class GETNAME : Command<BulkString>
        {
            private static readonly PlainBulkString subName = new PlainBulkString("GETNAME");
            private static readonly PlainBulkString[] request = {name, subName};
            public override IEnumerable<BulkString> Request(BulkStringFactory factory) => request;
            public override Visitor<BulkString> ResponseStructure => BulkStringExpectation.Singleton;
        }
    }
}