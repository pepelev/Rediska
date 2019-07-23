using System.Collections.Generic;
using Rediska.Protocol;
using Rediska.Protocol.Visitors;
using Rediska.Utils;

namespace Rediska.Commands.Keys
{
    public sealed class DEL : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("DEL");

        private readonly IReadOnlyList<Key> keys;

        public DEL(params Key[] keys)
            : this(keys as IReadOnlyList<Key>)
        {
        }

        public DEL(IReadOnlyList<Key> keys)
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