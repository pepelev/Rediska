namespace Rediska.Commands.Keys
{
    using System.Collections.Generic;
    using System.Globalization;
    using Protocol;
    using Protocol.Visitors;

    public sealed partial class SORT
    {
        // todo
        public sealed class STORE : Command<STORE.Response>
        {
            private static readonly PlainBulkString store = new PlainBulkString("STORE");
            private readonly Key destination;

            public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
            {
                name,
                destination.ToBulkString()
            };

            public override Visitor<Response> ResponseStructure { get; }

            public readonly struct Response
            {
                public Response(long storedElementCount)
                {
                    StoredElementCount = storedElementCount;
                }

                public long StoredElementCount { get; }
                public override string ToString() => StoredElementCount.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}