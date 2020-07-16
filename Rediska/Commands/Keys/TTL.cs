namespace Rediska.Commands.Keys
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class TTL : Command<TTL.Response>
    {
        private static readonly PlainBulkString name = new PlainBulkString("TTL");

        private static readonly Visitor<Response> responseStructure = IntegerExpectation.Singleton
            .Then(integer => new Response(integer));

        private readonly Key key;

        public TTL(Key key)
        {
            this.key = key;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory)
        };

        public override Visitor<Response> ResponseStructure => responseStructure;

        public readonly struct Response
        {
            public Response(long reply)
            {
                if (reply < -2)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(reply),
                        reply,
                        "Reply must be greater than or equal to -2"
                    );
                }

                Reply = reply;
            }

            public long Reply { get; }

            public long Seconds => Status == TtlStatus.Ok
                ? Reply
                : throw new InvalidOperationException(Status.ToString("G"));

            public TimeSpan TimeSpan => new TimeSpan(Seconds * TimeSpan.TicksPerSecond);

            public TtlStatus Status => Reply switch
            {
                -2 => TtlStatus.KeyNotExists,
                -1 => TtlStatus.NoExpiration,
                _ => TtlStatus.Ok
            };

            public override string ToString() => Status switch
            {
                TtlStatus.Ok => TimeSpan.ToString("c"),
                TtlStatus.KeyNotExists => "Key not exists",
                _ => "No expiration associated"
            };
        }
    }
}