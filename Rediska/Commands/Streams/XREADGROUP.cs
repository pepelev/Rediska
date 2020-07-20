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
        private readonly GroupName groupName;
        private readonly ConsumerName consumerName;
        private readonly Count count;
        private readonly Mode mode;
        private readonly IReadOnlyList<(Key Key, Id Id)> streams;

        public XREADGROUP(
            GroupName groupName,
            ConsumerName consumerName,
            Count count,
            Mode mode,
            params (Key Key, Id Id)[] streams)
            : this(groupName, consumerName, count, mode, streams as IReadOnlyList<(Key Key, Id Id)>)
        {
        }

        public XREADGROUP(
            GroupName groupName,
            ConsumerName consumerName,
            Count count,
            Mode mode,
            IReadOnlyList<(Key Key, Id Id)> streams)
        {
            if (count == null) throw new ArgumentNullException(nameof(count));
            if (streams == null) throw new ArgumentNullException(nameof(streams));
            ValidateMode(mode);
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
            {
                yield return noAck;
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