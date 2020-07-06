namespace Rediska.Commands.SortedSets
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class ZREM : Command<ZREM.Response>
    {
        private static readonly PlainBulkString name = new PlainBulkString("ZREM");

        private static readonly Visitor<Response> responseStructure = IntegerExpectation.Singleton
            .Then(membersRemoved => new Response(membersRemoved));

        private readonly Key key;
        private readonly IReadOnlyList<BulkString> members;

        public ZREM(Key key, params BulkString[] members)
            : this(key, members as IReadOnlyList<BulkString>)
        {
        }

        public ZREM(Key key, IReadOnlyList<BulkString> members)
        {
            this.key = key;
            this.members = members;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new ConcatList<BulkString>(
            new[]
            {
                name,
                key.ToBulkString(factory)
            },
            members
        );

        public override Visitor<Response> ResponseStructure => responseStructure;

        public readonly struct Response
        {
            public Response(long membersRemoved)
            {
                MembersRemoved = membersRemoved;
            }

            public long MembersRemoved { get; }
        }
    }
}