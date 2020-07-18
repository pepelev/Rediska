namespace Rediska.Commands.Streams
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Protocol;
    using Protocol.Visitors;

    public sealed class XDEL : Command<XDEL.Response>
    {
        private static readonly PlainBulkString name = new PlainBulkString("XDEL");

        private static readonly Visitor<Response> responseStructure = IntegerExpectation.Singleton
            .Then(entriesRemoved => new Response(entriesRemoved));

        private readonly Key key;
        private readonly IReadOnlyList<Id> entryIds;

        public XDEL(Key key, params Id[] entryIds)
            : this(key, entryIds as IReadOnlyList<Id>)
        {
        }

        public XDEL(Key key, IReadOnlyList<Id> entryIds)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));

            if (entryIds is null)
                throw new ArgumentNullException(nameof(entryIds));

            if (entryIds.Count < 1)
                throw new ArgumentException("Must contain items", nameof(entryIds));

            this.key = key;
            this.entryIds = entryIds;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory)
        {
            yield return name;
            yield return key.ToBulkString(factory);
            foreach (var entryId in entryIds)
            {
                yield return entryId.ToBulkString(factory, Id.Print.Full);
            }
        }

        public override Visitor<Response> ResponseStructure => responseStructure;

        public readonly struct Response
        {
            public Response(long entriesRemoved)
            {
                EntriesRemoved = entriesRemoved;
            }

            public long EntriesRemoved { get; }

            public override string ToString() =>
                $"{nameof(EntriesRemoved)}: {EntriesRemoved.ToString(CultureInfo.InvariantCulture)}";
        }
    }
}