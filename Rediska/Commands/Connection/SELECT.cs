namespace Rediska.Commands.Connection
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class SELECT : Command<None>
    {
        private static readonly PlainBulkString name = new PlainBulkString("SELECT");
        private readonly DatabaseNumber index;

        public SELECT(DatabaseNumber index)
        {
            this.index = index;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            index.ToBulkString(factory)
        };

        public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
    }
}