namespace Rediska.Commands.Hashes
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class HINCRBY : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("HINCRBY");
        private readonly Key key;
        private readonly BulkString field;
        private readonly long increment;

        public HINCRBY(Key key, BulkString field, long increment)
        {
            this.key = key;
            this.field = field;
            this.increment = increment;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory),
            field,
            factory.Create(increment)
        };

        public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;
    }
}