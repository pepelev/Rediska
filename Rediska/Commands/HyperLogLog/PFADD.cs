namespace Rediska.Commands.HyperLogLog
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class PFADD : Command<PFADD.Response>
    {
        private readonly Key key;
        private readonly IReadOnlyList<BulkString> values;

        public PFADD(Key key, params BulkString[] values)
            : this(key, values as IReadOnlyList<BulkString>)
        {
        }

        public PFADD(Key key, IReadOnlyList<BulkString> values)
        {
            this.key = key;
            this.values = values;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new PrefixedList<BulkString>(
            key.ToBulkString(),
            values
        );

        public override Visitor<Response> ResponseStructure => new ProjectingVisitor<long, Response>(
            IntegerExpectation.Singleton,
            integer => integer == 0
                ? Response.CardinalityChanged
                : Response.CardinalityNotChanged
        );

        public enum Response : byte
        {
            CardinalityNotChanged,
            CardinalityChanged
        }
    }
}