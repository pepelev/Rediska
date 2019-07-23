using System.Collections.Generic;
using Rediska.Protocol.Requests;
using Rediska.Protocol.Responses.Visitors;
using Rediska.Utils;

namespace Rediska.Commands.Sets
{
    public sealed class SREM : Command<long>
    {
        private static readonly BulkString name = new BulkString("SREM");

        private readonly Key key;
        private readonly IReadOnlyCollection<BulkString> members;

        public SREM(Key key, params BulkString[] members)
            : this(key, members as IReadOnlyCollection<BulkString>)
        {
        }

        public SREM(Key key, IReadOnlyCollection<BulkString> members)
        {
            this.key = key;
            this.members = members;
        }

        public override DataType Request => new Array(
            new ConcatCollection<DataType>(
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