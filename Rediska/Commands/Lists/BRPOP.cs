namespace Rediska.Commands.Lists
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class BRPOP : Command<PopResult>
    {
        private static readonly PlainBulkString name = new PlainBulkString("BRPOP");
        private readonly IReadOnlyList<Key> keys;
        private readonly Timeout timeout;

        public BRPOP(Key key, Timeout timeout)
            : this(new[] {key}, timeout)
        {
        }

        public BRPOP(IReadOnlyList<Key> keys, Timeout timeout)
        {
            this.keys = keys;
            this.timeout = timeout;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory)
        {
            yield return name;
            foreach (var key in keys)
            {
                yield return key.ToBulkString(factory);
            }

            yield return timeout.ToBulkString(factory);
        }

        public override Visitor<PopResult> ResponseStructure => CompositeVisitors.Pop;
    }
}