namespace Rediska.Commands.Server
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class SWAPDB : Command<None>
    {
        private static readonly PlainBulkString name = new PlainBulkString("SWAPDB");
        private readonly DatabaseNumber db1;
        private readonly DatabaseNumber db2;

        public SWAPDB(DatabaseNumber db1, DatabaseNumber db2)
        {
            this.db1 = db1;
            this.db2 = db2;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            factory.Create(db1.Value),
            factory.Create(db2.Value)
        };

        public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
    }
}