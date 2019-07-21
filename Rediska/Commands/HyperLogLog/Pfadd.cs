namespace Rediska.Commands.HyperLogLog
{
    public sealed class PFADD : Command<PFADD.Response>
    {
        public enum Response : byte
        {
            CardinalityNotChanged,
            CardinalityChanged
        }

        private readonly Key key;
        private readonly IReadOnlyCollection<BulkString> values;

        public PFADD(Key key, IReadOnlyCollection<BulkString> values)
        {
            this.key = key;
            this.values = values;
        }

        public PFADD(Key key, params BulkString[] values)
            : this(key, values as IReadOnlyCollection<BulkString>)
        {
        }

        public override DataType Request => new Array(
            new PrefixedCollection<DataType>(
                key.ToBulkString(),
                values
            )
        );

        public override Visitor<Response> ResponseStructure => new ProjectingVisitor<long, Response>(
            IntegerExpectation.Singleton,
            integer => integer == 0
                ? Response.CardinalityChanged
                : Response.CardinalityNotChanged
        );
    }
}