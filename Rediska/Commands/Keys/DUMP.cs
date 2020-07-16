namespace Rediska.Commands.Keys
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class DUMP : Command<BulkString>
    {
        private static readonly PlainBulkString name = new PlainBulkString("DUMP");
        private readonly Key key;

        public DUMP(Key key)
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