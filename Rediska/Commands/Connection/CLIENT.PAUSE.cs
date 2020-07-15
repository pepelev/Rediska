namespace Rediska.Commands.Connection
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public static partial class CLIENT
    {
        public sealed class PAUSE : Command<None>
        {
            private static readonly PlainBulkString subName = new PlainBulkString("PAUSE");

            // todo try to pass infinite timeout
            private readonly MillisecondsTimeout timeout;

            public PAUSE(MillisecondsTimeout timeout)
            {
                this.timeout = timeout;
            }

            public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
            {
                name,
                subName,
                timeout.ToBulkString(factory)
            };

            public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
        }
    }
}