using System.Collections.Generic;
using Rediska.Protocol;
using Rediska.Protocol.Visitors;
using Rediska.Utils;

namespace Rediska.Commands.Sets
{
    public sealed class SADD : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("SADD");

        private readonly Key key;
        private readonly IReadOnlyList<BulkString> members;

        public SADD(Key key, params BulkString[] members)
            : this(key, members as IReadOnlyList<BulkString>)
        {
        }

        public SADD(Key key, IReadOnlyList<BulkString> members)
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