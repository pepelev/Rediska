namespace Rediska.Commands.Keys
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class RENAMENX : Command<None>
    {
        private static readonly PlainBulkString name = new PlainBulkString("RENAMENX");
        private readonly Key key;
        private readonly Key newKey;

        public RENAMENX(Key key, Key newKey)
        {
            this.key = key;
            this.newKey = newKey;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(),
            newKey.ToBulkString()
        };

        // todo parse value and error
        public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
    }
}