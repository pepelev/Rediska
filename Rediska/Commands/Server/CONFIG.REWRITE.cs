namespace Rediska.Commands.Server
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public static partial class CONFIG
    {
        public sealed class REWRITE : Command<None>
        {
            private static readonly PlainBulkString subName = new PlainBulkString("REWRITE");
            private static readonly BulkString[] request = {name, subName};
            public override IEnumerable<BulkString> Request(BulkStringFactory factory) => request;
            public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
        }
    }
}