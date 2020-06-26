namespace Rediska.Commands.Keys
{
    using System;
    using Protocol;
    using Protocol.Visitors;

    public sealed class TTL : Command<TTL.Result>
    {
        public enum Status
        {
            KeyNotExists,
            NoExpiration,
            Ok
        }

        private static readonly PlainBulkString name = new PlainBulkString("TTL");
        private static readonly Visitor<Result> responseStructure = IntegerExpectation.Singleton.Then(integer => new Result(integer));
        private readonly Key key;

        public TTL(Key key)
        {
            this.key = key;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString()
        );

        public override Visitor<Result> ResponseStructure => responseStructure;

        public readonly struct Result
        {
            public Result(long reply)
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

            public long Seconds => Status == Status.Ok
                ? Reply
                : throw new InvalidOperationException(Status.ToString("G"));

            public TimeSpan TimeSpan => new TimeSpan(Seconds * TimeSpan.TicksPerSecond);

            public Status Status
            {
                get
                {
                    switch (Reply)
                    {
                        case -2:
                            return Status.KeyNotExists;
                        case -1:
                            return Status.NoExpiration;
                        default:
                            return Status.Ok;
                    }
                }
            }
        }
    }
}