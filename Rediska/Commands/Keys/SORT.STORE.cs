﻿namespace Rediska.Commands.Keys
{
    using System.Globalization;
    using Protocol;
    using Protocol.Visitors;

    public sealed partial class SORT
    {
        public sealed class STORE : Command<STORE.Response>
        {
            public readonly struct Response
            {
                public Response(long storedElementCount)
                {
                    StoredElementCount = storedElementCount;
                }

                public long StoredElementCount { get; }
                public override string ToString() => StoredElementCount.ToString(CultureInfo.InvariantCulture);
            }

            private static readonly PlainBulkString store = new PlainBulkString("STORE");
            private readonly Key destination;

            public override DataType Request => new PlainArray(
                name,
                destination.ToBulkString()
            );

            public override Visitor<Response> ResponseStructure { get; }
        }
    }
}