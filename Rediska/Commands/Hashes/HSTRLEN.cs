namespace Rediska.Commands.Hashes
{
    using System.Globalization;
    using Protocol;
    using Protocol.Visitors;

    public sealed class HSTRLEN : Command<HSTRLEN.Response>
    {
        public readonly struct Response
        {
            public Response(long fieldLength)
            {
                FieldLength = fieldLength;
            }

            public long FieldLength { get; }
            public bool HashAndFieldExists => FieldLength > 0;

            public override string ToString()
            {
                return HashAndFieldExists
                    ? FieldLength.ToString(CultureInfo.InvariantCulture)
                    : "hash or field does not exists";
            }
        }

        private static readonly PlainBulkString name = new PlainBulkString("HSTRLEN");
        private readonly Key key;
        private readonly Key field;

        public HSTRLEN(Key key, Key field)
        {
            this.key = key;
            this.field = field;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString(),
            field.ToBulkString()
        );

        public override Visitor<Response> ResponseStructure => IntegerExpectation.Singleton.Then(integer => new Response(integer));
    }
}