namespace Rediska.Commands.Lists
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class LPOP : Command<BulkString>
    {
        private static readonly PlainBulkString name = new PlainBulkString("LPOP");
        private readonly Key key;

        public LPOP(Key key)
        {
            this.key = key;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory)
        };

        public override Visitor<BulkString> ResponseStructure => BulkStringExpectation.Singleton;
    }
}