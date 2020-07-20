namespace Rediska.Commands.Streams
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed partial class XREADGROUP : Command<IReadOnlyList<Entries>>
    {
        private static readonly PlainBulkString name = new PlainBulkString("XREADGROUP");
        private static readonly PlainBulkString group = new PlainBulkString("GROUP");
        private static readonly PlainBulkString streamsArgument = new PlainBulkString("STREAMS");
        private static readonly PlainBulkString noAck = new PlainBulkString("NOACK");
        private static readonly PlainBulkString greaterThan = new PlainBulkString(">");
        private readonly GroupName groupName;
        private readonly ConsumerName consumerName;
        private readonly Count count;
        private readonly IReadOnlyList<(Key Key, Id Id)> streams;

        public XREADGROUP(
            GroupName groupName,
            ConsumerName consumerName,
            Count count,
            params (Key Key, Id Id)[] streams)
            : this(groupName, consumerName, count, streams as IReadOnlyList<(Key Key, Id Id)>)
        {
        }

        public XREADGROUP(
            GroupName groupName,
            ConsumerName consumerName,
            Count count,
            IReadOnlyList<(Key Key, Id Id)> streams)
        {
            this.groupName = groupName;
            this.consumerName = consumerName;
            this.count = count ?? throw new ArgumentNullException(nameof(count));
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

            yield return streamsArgument;
            foreach (var (key, _) in streams)
            {
                yield return key.ToBulkString(factory);
            }

            foreach (var (_, id) in streams)
            {
                yield return id.ToBulkString(factory, Id.Print.SkipMinimalLow);
            }
        }

        public override Visitor<IReadOnlyList<Entries>> ResponseStructure => CompositeVisitors.StreamEntriesList;

        private static void ValidateMode(Mode mode)
        {
            switch (mode)
            {
                case Mode.RequireAcknowledgment:
                case Mode.NotRequireAcknowledgment:
                    break;
                default:
                {
                    throw new ArgumentException(
                        $"Must be either RequireAcknowledgment or NotRequireAcknowledgment, but {mode} found",
                        nameof(mode)
                    );
                }
            }
        }

        public enum Mode
        {
            RequireAcknowledgment = 0,
            NotRequireAcknowledgment = 1
        }
    }
}