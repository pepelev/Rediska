namespace Rediska.Commands.Lists
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class BRPOPLPUSH : Command<BulkString>
    {
        private static readonly PlainBulkString name = new PlainBulkString("BRPOPLPUSH");
        private readonly Key source;
        private readonly Key destination;
        private readonly Timeout timeout;

        public BRPOPLPUSH(Key source, Key destination, Timeout timeout)
        {
            this.source = source;
            this.destination = destination;
            this.timeout = timeout;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            source.ToBulkString(factory),
            destination.ToBulkString(factory),
            timeout.ToBulkString(factory)
        };

        public override Visitor<BulkString> ResponseStructure => BulkStringExpectation.Singleton;
    }
}