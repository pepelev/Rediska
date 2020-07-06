namespace Rediska.Commands.Hashes
{
    using System.Collections.Generic;
    using System.Globalization;
    using Protocol;
    using Protocol.Visitors;

    public sealed class HSTRLEN : Command<HSTRLEN.Response>
    {
        private static readonly PlainBulkString name = new PlainBulkString("HSTRLEN");
        private readonly Key key;
        private readonly BulkString field;

        public HSTRLEN(Key key, BulkString field)
        {
            this.key = key;
            this.field = field;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory),
            field
        };

        public override Visitor<Response> ResponseStructure => IntegerExpectation.Singleton.Then(integer => new Response(integer));

        public readonly struct Response
        {
            public Response(long fieldLength)
            {
                FieldLength = fieldLength;
            }

            public long FieldLength { get; }
            public bool HashAndFieldExists => FieldLength > 0;

            public override string ToString() => HashAndFieldExists
                ? $"{nameof(FieldLength)}: {FieldLength}"
                : "hash or field does not exists";
        }
    }
}