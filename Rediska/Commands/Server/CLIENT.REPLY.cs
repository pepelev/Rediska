namespace Rediska.Commands.Server
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public static partial class CLIENT
    {
        public sealed class REPLY : Command<None>
        {
            private static readonly PlainBulkString subName = new PlainBulkString("REPLY");
            public static REPLY ON { get; } = new REPLY("ON");
            public static REPLY OFF { get; } = new REPLY("OFF");
            public static REPLY SKIP { get; } = new REPLY("SKIP");
            private readonly BulkString[] request;

            public REPLY(BulkString argument)
            {
                request = new[] {name, subName, argument};
            }

            public override IEnumerable<BulkString> Request(BulkStringFactory factory) => request;
            public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
        }
    }
}