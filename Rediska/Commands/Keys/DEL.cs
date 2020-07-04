namespace Rediska.Commands.Keys
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class DEL : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("DEL");
        private readonly IReadOnlyList<Key> keys;

        public DEL(params Key[] keys)
            : this(keys as IReadOnlyList<Key>)
        {
        }

        public DEL(IReadOnlyList<Key> keys)
        {
            this.keys = keys;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new PrefixedList<BulkString>(
            name,
            new KeyList(keys)
        );

        public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;
    }
}