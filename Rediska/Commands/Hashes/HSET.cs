namespace Rediska.Commands.Hashes
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class HSET : Command<HSET.Response>
    {
        private static readonly PlainBulkString name = new PlainBulkString("HSET");

        private static readonly Visitor<Response> responseStructure = IntegerExpectation.Singleton
            .Then(keysAdded => new Response(keysAdded));

        private readonly Key key;
        private readonly IReadOnlyList<(BulkString Field, BulkString Value)> pairs;

        public HSET(Key key, BulkString field, BulkString value)
            : this(key, (field, value))
        {
        }

        public HSET(Key key, params (BulkString Field, BulkString Value)[] pairs)
            : this(key, pairs as IReadOnlyList<(BulkString Field, BulkString Value)>)
        {
        }

        public HSET(Key key, IReadOnlyList<(BulkString Key, BulkString Value)> pairs)
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

        public override Visitor<Response> ResponseStructure => responseStructure;

        public readonly struct Response
        {
            public Response(long keysAdded)
            {
                KeysAdded = keysAdded;
            }

            public long KeysAdded { get; }
            public override string ToString() => $"{nameof(KeysAdded)}: {KeysAdded}";
        }
    }
}