namespace Rediska.Commands.Keys
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class EXISTS : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("EXISTS");
        private readonly IReadOnlyList<Key> keys;

        public EXISTS(params Key[] keys)
            : this(keys as IReadOnlyList<Key>)
        {
            this.keys = keys;
        }

        public EXISTS(IReadOnlyList<Key> keys)
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