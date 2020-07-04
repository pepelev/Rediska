namespace Rediska.Commands.Server
{
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

        public override DataType Request => new PlainArray(
            name,
            db1.ToBulkString(),
            db2.ToBulkString()
        );

        public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
    }
}