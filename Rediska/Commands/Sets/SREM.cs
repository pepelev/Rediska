using System.Collections.Generic;
using Rediska.Protocol;
using Rediska.Protocol.Visitors;
using Rediska.Utils;

namespace Rediska.Commands.Sets
{
    public sealed class SREM : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("SREM");

        private readonly Key key;
        private readonly IReadOnlyList<BulkString> members;

        public SREM(Key key, params BulkString[] members)
            : this(key, members as IReadOnlyList<BulkString>)
        {
        }

        public SREM(Key key, IReadOnlyList<BulkString> members)
        {
            this.key = key;
            this.members = members;
        }

        public override DataType Request => new PlainArray(
            new ConcatList<DataType>(
                new DataType[]
                {
                    name,
                    key.ToBulkString()
                },
                members
            )
        );

        public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;
    }
}