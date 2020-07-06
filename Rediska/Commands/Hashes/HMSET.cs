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
        private readonly IReadOnlyList<(BulkString Field, BulkString Value)> pairs;

        public HMSET(Key key, params (BulkString Field, BulkString Value)[] pairs)
            : this(key, pairs as IReadOnlyList<(BulkString Field, BulkString Value)>)
        {
        }

        public HMSET(Key key, IReadOnlyList<(BulkString Key, BulkString Value)> pairs)
        {
            this.key = key;
            this.pairs = pairs;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory)
        {
            yield return name;
            yield return key.ToBulkString(factory);
            foreach (var (field, value) in pairs)
            {
                yield return field;
                yield return value;
            }
        }

        public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
    }
}