namespace Rediska.Commands.Strings
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class PSETEX : Command<None>
    {
        private static readonly PlainBulkString name = new PlainBulkString("PSETEX");
        private readonly Key key;
        private readonly long milliseconds;
        private readonly BulkString value;

        public PSETEX(Key key, long milliseconds, BulkString value)
        {
            this.key = key;
            this.milliseconds = milliseconds;
            this.value = value;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory),
            factory.Create(milliseconds),
            value
        };

        public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
    }
}