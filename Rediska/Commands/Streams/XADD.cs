namespace Rediska.Commands.Streams
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Array = System.Array;

    public sealed class XADD : Command<XADD.Response>
    {
        private static readonly PlainBulkString name = new PlainBulkString("XADD");
        private readonly Key key;
        private readonly Trim trim;
        private readonly Id id;
        private readonly IReadOnlyList<(BulkString Field, BulkString Value)> members;

        public XADD(Key key, params (BulkString Field, BulkString Value)[] members)
            : this(key, members as IReadOnlyList<(BulkString Field, BulkString Value)>)
        {
        }

        public XADD(Key key, IReadOnlyList<(BulkString Field, BulkString Value)> members)
            : this(key, Trim.None, Id.Auto, members)
        {
        }

        public XADD(Key key, Trim trim, Id id, params (BulkString Field, BulkString Value)[] members)
            : this(key, trim, id, members as IReadOnlyList<(BulkString Field, BulkString Value)>)
        {
        }

        public XADD(Key key, Trim trim, Id id, IReadOnlyList<(BulkString Field, BulkString Value)> members)
        {
            this.key = key ?? throw new ArgumentNullException(nameof(key));
            this.trim = trim ?? throw new ArgumentNullException(nameof(trim));
            this.id = id ?? throw new ArgumentNullException(nameof(id));
            this.members = members ?? throw new ArgumentNullException(nameof(members));

            if (members.Count < 1)
                throw new ArgumentException("Must contain elements", nameof(members));
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory)
        {
            yield return name;
            yield return key.ToBulkString(factory);
            foreach (var argument in trim.Arguments(factory))
            {
                yield return argument;
            }

            yield return id.ToBulkString(factory);
            foreach (var (field, value) in members)
            {
                yield return field;
                yield return value;
            }
        }

        public override Visitor<Response> ResponseStructure => BulkStringExpectation.Singleton.Then(
            bulkString =>
                new Response(
                    Streams.Id.Parse(bulkString.ToString())
                )
        );

        public readonly struct Response
        {
            public Response(Streams.Id addedEntryId)
            {
                AddedEntryId = addedEntryId;
            }

            public Streams.Id AddedEntryId { get; }
            public override string ToString() => $"{nameof(AddedEntryId)}: {AddedEntryId}";
        }

        public abstract class Trim
        {
            public static Trim None { get; } = new NoTrim();
            public abstract BulkString[] Arguments(BulkStringFactory factory);

            private sealed class NoTrim : Trim
            {
                public override BulkString[] Arguments(BulkStringFactory factory) => Array.Empty<BulkString>();
            }
        }

        public sealed class MaximumLength : Trim
        {
            private readonly MaximumLengthTrim trim;

            public MaximumLength(MaximumLengthTrim trim)
            {
                this.trim = trim;
            }

            public override BulkString[] Arguments(BulkStringFactory factory) => trim.Arguments(factory);
        }

        public abstract class Id
        {
            public static Id Auto { get; } = new AutoId();
            public static implicit operator Id(Streams.Id id) => new Exact(id);
            public abstract BulkString ToBulkString(BulkStringFactory factory);

            private sealed class AutoId : Id
            {
                private static readonly PlainBulkString star = new PlainBulkString("*");
                public override BulkString ToBulkString(BulkStringFactory factory) => star;
            }

            public sealed class Exact : Id
            {
                private readonly Streams.Id value;

                public Exact(Streams.Id value)
                {
                    this.value = value;
                }

                public override BulkString ToBulkString(BulkStringFactory factory) => value.ToBulkString(factory, Streams.Id.Print.Full);
            }
        }
    }
}