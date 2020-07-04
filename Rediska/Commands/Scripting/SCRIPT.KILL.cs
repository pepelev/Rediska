namespace Rediska.Commands.Scripting
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public static partial class SCRIPT
    {
        public sealed class KILL : Command<None>
        {
            private static readonly PlainBulkString subName = new PlainBulkString("KILL");
            private static readonly BulkString[] request = {name, subName};
            public static KILL Singleton { get; } = new KILL();
            public override IEnumerable<BulkString> Request(BulkStringFactory factory) => request;
            public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
        }
    }
}