namespace Rediska.Commands.Hashes
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class HKEYS : Command<IReadOnlyList<Key>>
    {
        private static readonly PlainBulkString name = new PlainBulkString("HKEYS");
        private readonly Key key;

        public HKEYS(Key key)
        {
            this.key = key;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString()
        );

        public override Visitor<IReadOnlyList<Key>> ResponseStructure => CompositeVisitors.KeyList;
    }
}