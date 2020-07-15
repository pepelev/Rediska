namespace Rediska.Commands.Connection
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public static partial class CLIENT
    {
        public sealed class SETNAME : Command<None>
        {
            private static readonly PlainBulkString subName = new PlainBulkString("SETNAME");
            private readonly string connectionName;

            // todo try to pass space
            public SETNAME(string connectionName)
            {
                this.connectionName = connectionName;
            }

            public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
            {
                name,
                subName,
                factory.Utf8(connectionName)
            };

            public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
        }
    }
}