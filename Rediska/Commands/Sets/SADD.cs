using System.Collections.Generic;
using Rediska.Protocol.Requests;
using Rediska.Protocol.Responses.Visitors;
using Rediska.Utils;

namespace Rediska.Commands.Sets
{
    public sealed class SADD : Command<long>
    {
        private static readonly BulkString name = new BulkString("SADD");

        private readonly Key key;
        private readonly IReadOnlyCollection<BulkString> members;

        public SADD(Key key, IReadOnlyCollection<BulkString> members)
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