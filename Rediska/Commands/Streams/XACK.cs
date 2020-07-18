namespace Rediska.Commands.Streams
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Protocol;
    using Protocol.Visitors;

    public sealed class XACK : Command<XACK.Response>
    {
        private static readonly PlainBulkString name = new PlainBulkString("XACK");

        private static readonly Visitor<Response> responseStructure = IntegerExpectation.Singleton
            .Then(entriesAcknowledged => new Response(entriesAcknowledged));

        private readonly Key key;
        private readonly GroupName groupName;
        private readonly IReadOnlyList<Id> ids;

        public XACK(Key key, GroupName groupName, params Id[] ids)
            : this(key, groupName, ids as IReadOnlyList<Id>)
        {
        }

        public XACK(Key key, GroupName groupName, IReadOnlyList<Id> ids)
        {
            this.key = key ?? throw new ArgumentNullException(nameof(key));
            this.groupName = groupName;
            this.ids = ids ?? throw new ArgumentNullException(nameof(ids));

            if (ids.Count < 1)
                throw new ArgumentException("Must contain elements", nameof(ids));
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory)
        {
            yield return name;
            key.ToBulkString(factory);
            groupName.ToBulkString(factory);

            foreach (var id in ids)
            {
                id.ToBulkString(factory, Id.Print.Full);
            }
        }

        public override Visitor<Response> ResponseStructure => responseStructure;

        public readonly struct Response
        {
            public Response(long entriesAcknowledged)
            {
                EntriesAcknowledged = entriesAcknowledged;
            }

            public long EntriesAcknowledged { get; }

            public override string ToString() =>
                $"{nameof(EntriesAcknowledged)}: {EntriesAcknowledged.ToString(CultureInfo.InvariantCulture)}";
        }
    }
}