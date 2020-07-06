namespace Rediska.Commands.Server
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed partial class COMMAND
    {
        public sealed class COUNT : Command<long>
        {
            private static readonly PlainBulkString subName = new PlainBulkString("COUNT");
            private static readonly BulkString[] request = {name, subName};
            public static COUNT Singleton { get; } = new COUNT();
            public override IEnumerable<BulkString> Request(BulkStringFactory factory) => request;
            public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;
        }
    }
}