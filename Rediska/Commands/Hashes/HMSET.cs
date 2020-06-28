namespace Rediska.Commands.Hashes
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class HMSET : Command<None>
    {
        private static readonly PlainBulkString name = new PlainBulkString("HMSET");
        private readonly Key key;
        private readonly IReadOnlyList<(Key Field, BulkString Value)> pairs;

        public HMSET(Key key, Key field, BulkString value)
            : this(key, (field, value))
        {
        }

        public HMSET(Key key, params (Key Field, BulkString Value)[] pairs)
            : this(key, pairs as IReadOnlyList<(Key Field, BulkString Value)>)
        {
        }

        public HMSET(Key key, IReadOnlyList<(Key Key, BulkString Value)> pairs)
        {
            this.key = key;
            this.pairs = pairs;
        }

        public override DataType Request => new PlainArray(
            new SetRequest(name, key, pairs)
        );

        public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
    }
}