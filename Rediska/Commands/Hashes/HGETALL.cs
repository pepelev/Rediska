namespace Rediska.Commands.Hashes
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class HGETALL : Command<IReadOnlyList<HashEntry>>
    {
        private static readonly BulkString name = new PlainBulkString("HGETALL");
        private readonly Key key;

        public HGETALL(Key key)
        {
            this.key = key;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString()
        );

        public override Visitor<IReadOnlyList<HashEntry>> ResponseStructure => CompositeVisitors.HashEntryList;
    }
}