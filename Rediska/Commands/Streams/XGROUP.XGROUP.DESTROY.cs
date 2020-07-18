namespace Rediska.Commands.Streams
{
    using System.Collections.Generic;
    using System.Globalization;
    using Protocol;
    using Protocol.Visitors;

    public static partial class XGROUP
    {
        public sealed class DESTROY : Command<DESTROY.Response>
        {
            private static readonly PlainBulkString subName = new PlainBulkString("DESTROY");
            private readonly Key key;
            private readonly GroupName groupName;

            public DESTROY(Key key, GroupName groupName)
            {
                this.key = key;
                this.groupName = groupName;
            }

            public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
            {
                name,
                subName,
                key.ToBulkString(factory),
                groupName.ToBulkString(factory)
            };

            public override Visitor<Response> ResponseStructure => IntegerExpectation.Singleton
                .Then(reply => new Response(reply));

            public readonly struct Response
            {
                public Response(long rawReply)
                {
                    RawReply = rawReply;
                }

                public long RawReply { get; }
                public bool GroupDeleted => RawReply > 0;

                public override string ToString() =>
                    $"{nameof(GroupDeleted)}: {GroupDeleted.ToString(CultureInfo.InvariantCulture)}";
            }
        }
    }
}