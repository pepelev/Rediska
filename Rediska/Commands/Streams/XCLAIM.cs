namespace Rediska.Commands.Streams
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class XCLAIM : Command<IReadOnlyList<Entry>>
    {
        private static readonly PlainBulkString name = new PlainBulkString("XCLAIM");
        private readonly Key key;
        private readonly GroupName groupName;
        private readonly ConsumerName consumerName;
        private readonly Milliseconds minimumIdleTime;
        private readonly IReadOnlyList<Id> ids;

        public XCLAIM(
            Key key,
            GroupName groupName,
            ConsumerName consumerName,
            Milliseconds minimumIdleTime,
            IReadOnlyList<Id> ids)
        {
            if (ids == null) throw new ArgumentNullException(nameof(ids));
            if (ids.Count < 1)
                throw new ArgumentException("Must contain items", nameof(ids));

            this.key = key ?? throw new ArgumentNullException(nameof(key));
            this.groupName = groupName;
            this.consumerName = consumerName;
            this.minimumIdleTime = minimumIdleTime;
            this.ids = ids;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory)
        {
            yield return name;
            yield return key.ToBulkString(factory);
            yield return groupName.ToBulkString(factory);
            yield return consumerName.ToBulkString(factory);
            yield return minimumIdleTime.ToBulkString(factory);
            foreach (var id in ids)
            {
                yield return id.ToBulkString(factory, Id.Print.Full);
            }
        }

        public override Visitor<IReadOnlyList<Entry>> ResponseStructure => RangeConstants.ResponseStructure;
    }
}