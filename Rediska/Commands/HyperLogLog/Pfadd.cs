using System.Collections.Generic;
using Rediska.Protocol;
using Rediska.Protocol.Visitors;
using Rediska.Utils;

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
        private readonly IReadOnlyList<BulkString> values;

        public PFADD(Key key, IReadOnlyList<BulkString> values)
        {
            this.key = key;
            this.values = values;
        }

        public PFADD(Key key, params BulkString[] values)
            : this(key, values as IReadOnlyList<BulkString>)
        {
        }

        public override DataType Request => new PlainArray(
            new PrefixedList<DataType>(
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