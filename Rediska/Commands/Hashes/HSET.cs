namespace Rediska.Commands.Hashes
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class HSET : Command<HSET.Response>
    {
        private static readonly PlainBulkString name = new PlainBulkString("HSET");
        private readonly Key key;
        private readonly IReadOnlyList<(Key Field, BulkString Value)> pairs;

        public HSET(Key key, Key field, BulkString value)
            : this(key, (field, value))
        {
        }

        public HSET(Key key, params (Key Field, BulkString Value)[] pairs)
            : this(key, pairs as IReadOnlyList<(Key Field, BulkString Value)>)
        {
        }

        public HSET(Key key, IReadOnlyList<(Key Key, BulkString Value)> pairs)
        {
            this.key = key;
            this.pairs = pairs;
        }

        public override DataType Request => new PlainArray(
            new SetRequest(name, key, pairs)
        );

        public override Visitor<Response> ResponseStructure => IntegerExpectation.Singleton
            .Then(keysAdded => new Response(keysAdded));

        public readonly struct Response
        {
            public Response(long keysAdded)
            {
                KeysAdded = keysAdded;
            }

            public long KeysAdded { get; }
            public override string ToString() => KeysAdded.ToString();
        }
    }
}