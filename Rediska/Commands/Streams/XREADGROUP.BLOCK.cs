namespace Rediska.Commands.Streams
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed partial class XREADGROUP
    {
        public sealed class BLOCK : Command<XREAD.BLOCK.Response>
        {
            private static readonly PlainBulkString block = new PlainBulkString("BLOCK");
            private static readonly PlainBulkString greaterThan = new PlainBulkString(">");
            private readonly GroupName groupName;
            private readonly ConsumerName consumerName;
            private readonly Count count;
            private readonly MillisecondsTimeout blockTimeout;
            private readonly IReadOnlyList<Key> streams;

            public BLOCK(
                GroupName groupName,
                ConsumerName consumerName,
                Count count,
                MillisecondsTimeout blockTimeout,
                IReadOnlyList<Key> streams)
            {
                this.groupName = groupName;
                this.consumerName = consumerName;
                this.count = count ?? throw new ArgumentNullException(nameof(count));
                this.blockTimeout = blockTimeout;
                this.streams = streams ?? throw new ArgumentNullException(nameof(streams));
            }

            public override IEnumerable<BulkString> Request(BulkStringFactory factory)
            {
                yield return name;
                yield return group;
                yield return groupName.ToBulkString(factory);
                yield return consumerName.ToBulkString(factory);
                foreach (var argument in count.Arguments(factory))
                {
                    yield return argument;
                }

                yield return block;
                yield return blockTimeout.ToBulkString(factory);

                yield return streamsArgument;
                foreach (var key in streams)
                {
                    yield return key.ToBulkString(factory);
                }

                for (var i = 0; i < streams.Count; i++)
                {
                    yield return greaterThan;
                }
            }

            public override Visitor<XREAD.BLOCK.Response> ResponseStructure => CompositeVisitors.StreamBlockingRead;
        }
    }
}