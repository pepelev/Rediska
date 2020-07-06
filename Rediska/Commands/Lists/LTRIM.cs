namespace Rediska.Commands.Lists
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class LTRIM : Command<None>
    {
        private static readonly PlainBulkString name = new PlainBulkString("LTRIM");
        private readonly Key key;
        private readonly Index start;
        private readonly Index stop;

        public LTRIM(Key key, Index start, Index stop)
        {
            this.key = key;
            this.start = start;
            this.stop = stop;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory),
            start.ToBulkString(factory),
            stop.ToBulkString(factory)
        };

        public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
    }
}