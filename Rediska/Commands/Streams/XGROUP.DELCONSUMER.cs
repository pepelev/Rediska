﻿namespace Rediska.Commands.Streams
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Protocol;
    using Protocol.Visitors;

    public static partial class XGROUP
    {
        public sealed class DELCONSUMER : Command<DELCONSUMER.Response>
        {
            private static readonly PlainBulkString subName = new PlainBulkString("DELCONSUMER");

            private static readonly Visitor<Response> responseStructure = IntegerExpectation.Singleton
                .Then(pendingEntriesRemoved => new Response(pendingEntriesRemoved));

            private readonly Key key;
            private readonly GroupName groupName;
            private readonly ConsumerName consumerName;

            public DELCONSUMER(Key key, GroupName groupName, ConsumerName consumerName)
            {
                this.key = key ?? throw new ArgumentNullException(nameof(key));
                this.groupName = groupName;
                this.consumerName = consumerName;
            }

            public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
            {
                name,
                subName,
                key.ToBulkString(factory),
                groupName.ToBulkString(factory),
                consumerName.ToBulkString(factory)
            };

            public override Visitor<Response> ResponseStructure => responseStructure;

            public readonly struct Response
            {
                public Response(long pendingEntriesRemoved)
                {
                    PendingEntriesRemoved = pendingEntriesRemoved;
                }

                public long PendingEntriesRemoved { get; }

                public override string ToString() =>
                    $"{nameof(PendingEntriesRemoved)}: {PendingEntriesRemoved.ToString(CultureInfo.InvariantCulture)}";
            }
        }
    }
}