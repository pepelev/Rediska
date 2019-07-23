using System.Collections.Generic;
using Rediska.Protocol;
using Rediska.Protocol.Visitors;
using Rediska.Utils;

namespace Rediska.Commands.Hashes
{
    public sealed class HDEL : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("HDEL");

        private readonly Key key;
        private readonly IReadOnlyList<Key> fields;

        public HDEL(Key key, IReadOnlyList<Key> fields)
        {
            this.key = key;
            this.fields = fields;
        }

        public override DataType Request => new PlainArray(
            new ConcatList<DataType>(
                new DataType[]
                {
                    name,
                    key.ToBulkString()
                },
                new KeyList(fields)
            )
        );

        public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;
    }
}