namespace Rediska.Commands.Keys
{
    using System;
    using Protocol;
    using Protocol.Visitors;

    public sealed class PTTL : Command<PTTL.Response>
    {
        private static readonly PlainBulkString name = new PlainBulkString("PTTL");

        private static readonly Visitor<Response> responseStructure = IntegerExpectation.Singleton
            .Then(integer => new Response(integer));

        private readonly Key key;

        public PTTL(Key key)
        {
            this.key = key;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString()
        );

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

            public long Milliseconds => Status == TtlStatus.Ok
                ? Reply
                : throw new InvalidOperationException(Status.ToString("G"));

            public TimeSpan TimeSpan => new TimeSpan(Milliseconds * TimeSpan.TicksPerMillisecond);

            public TtlStatus Status => Reply switch
            {
                -2 => TtlStatus.KeyNotExists,
                -1 => TtlStatus.NoExpiration,
                _ => TtlStatus.Ok
            };
        }
    }
}