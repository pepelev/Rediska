namespace Rediska.Commands.Strings
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class GETSET : Command<BulkString>
    {
        private static readonly PlainBulkString name = new PlainBulkString("GETSET");
        private readonly Key key;
        private readonly BulkString value;

        public GETSET(Key key, BulkString value)
        {
            this.key = key;
            this.value = value;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory),
            value
        };

        public override Visitor<BulkString> ResponseStructure => BulkStringExpectation.Singleton;
    }
}