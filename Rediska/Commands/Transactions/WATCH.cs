using System.Collections.Generic;
using Rediska.Protocol;
using Rediska.Protocol.Visitors;
using Rediska.Utils;

namespace Rediska.Commands.Transactions
{
    public sealed class WATCH : Command<None>
    {
        private static readonly PlainBulkString name = new PlainBulkString("WATCH");

        private readonly IReadOnlyList<Key> keys;

        public WATCH(params Key[] keys)
            : this(keys as IReadOnlyList<Key>)
        {
        }

        public WATCH(IReadOnlyList<Key> keys)
        {
            this.keys = keys;
        }

        public override DataType Request => new PlainArray(
            new PrefixedList<DataType>(
                name,
                new KeyList(keys)
            )
        );

        public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
    }
}