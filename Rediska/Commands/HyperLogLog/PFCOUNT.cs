using System.Collections.Generic;
using Rediska.Protocol;
using Rediska.Protocol.Visitors;
using Rediska.Utils;

namespace Rediska.Commands.HyperLogLog
{
    public sealed class PFCOUNT : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("PFCOUNT");

        private readonly IReadOnlyList<Key> keys;

        public PFCOUNT(params Key[] keys)
            : this(keys as IReadOnlyList<Key>)
        {
        }

        public PFCOUNT(IReadOnlyList<Key> keys)
        {
            this.keys = keys;
        }

        public override DataType Request => new PlainArray(
            new PrefixedList<DataType>(
                name,
                new KeyList(keys)
            )
        );

        public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;
    }
}