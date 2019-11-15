namespace Rediska.Commands.Lists
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class BLPOP : Command<PopResult>
    {
        private static readonly PlainBulkString name = new PlainBulkString("BLPOP");
        private readonly IReadOnlyList<Key> keys;
        private readonly Timeout timeout;

        public BLPOP(Key key, Timeout timeout)
            : this(new[] {key}, timeout)
        {
        }

        public BLPOP(IReadOnlyList<Key> keys, Timeout timeout)
        {
            this.keys = keys;
            this.timeout = timeout;
        }

        public override DataType Request => new PlainArray(
            // todo специализированная версия листа с листом посередине
            new ConcatList<DataType>(
                new PrefixedList<DataType>(
                    name,
                    new KeyList(keys)
                ),
                new[] {timeout.ToBulkString()}
            )
        );

        public override Visitor<PopResult> ResponseStructure => CompositeVisitors.Pop;
    }
}