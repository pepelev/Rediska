namespace Rediska.Commands.Streams
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed partial class XREADGROUP
    {
        public sealed class New : Command<IReadOnlyList<Entries>>
        {
            private readonly GroupName groupName;
            private readonly ConsumerName consumerName;
            private readonly Count count;
            private readonly Mode mode;
            private readonly IReadOnlyList<Key> streams;

            public New(
                GroupName groupName,
                ConsumerName consumerName,
                Count count,
                Mode mode,
                params Key[] streams)
                : this(groupName, consumerName, count, mode, streams as IReadOnlyList<Key>)
            {
            }

            public New(
                GroupName groupName,
                ConsumerName consumerName,
                Count count,
                Mode mode,
                IReadOnlyList<Key> streams)
            {
                this.groupName = groupName;
                this.consumerName = consumerName;
                this.count = count;
                this.mode = mode;
                this.streams = streams;
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

                if (mode == Mode.NotRequireAcknowledgment)
                    yield return noAck;

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

            public override Visitor<IReadOnlyList<Entries>> ResponseStructure => CompositeVisitors.StreamEntriesList;
        }
    }
}