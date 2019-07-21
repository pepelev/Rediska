using System.Collections.Generic;
using Rediska.Protocol.Requests;
using Rediska.Protocol.Responses.Visitors;
using Rediska.Utils;

namespace Rediska.Commands.Hashes
{
    public sealed class HDEL : Command<long>
    {
        private static readonly BulkString name = new BulkString("HDEL");

        private readonly Key key;
        private readonly IReadOnlyCollection<Key> fields;

        public HDEL(Key key, IReadOnlyCollection<Key> fields)
        {
            this.key = key;
            this.fields = fields;
        }

        public override DataType Request => new Array(
            new ConcatCollection<DataType>(
                new DataType[]
                {
                    name,
                    key.ToBulkString()
                },
                new ProjectingCollection<Key, DataType>(
                    fields,
                    field => field.ToBulkString()
                )
            )
        );

        public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;
    }
}