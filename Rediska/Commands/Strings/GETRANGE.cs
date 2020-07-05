namespace Rediska.Commands.Strings
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class GETRANGE : Command<BulkString>
    {
        private static readonly PlainBulkString name = new PlainBulkString("GETRANGE");
        private readonly Key key;
        private readonly Range range;

        public GETRANGE(Key key, Range range)
        {
            this.key = key;
            this.range = range;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory),
            range.StartInclusive.ToBulkString(factory),
            range.EndInclusive.ToBulkString(factory)
        };

        public override Visitor<BulkString> ResponseStructure => BulkStringExpectation.Singleton;
    }
}