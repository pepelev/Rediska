namespace Rediska.Commands.Server
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class BGSAVE : Command<BGSAVE.Response>
    {
        private static readonly PlainBulkString name = new PlainBulkString("BGSAVE");
        private static readonly BulkString[] immediateRequest = {name};

        private static readonly BulkString[] scheduleRequest =
        {
            name,
            new PlainBulkString("SCHEDULE")
        };

        private readonly Mode mode;

        public BGSAVE(Mode mode = Mode.Immediately)
        {
            if (mode != Mode.Immediately && mode != Mode.Schedule)
                throw new ArgumentException("Must be either Immediately or Schedule", nameof(mode));

            this.mode = mode;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => mode == Mode.Immediately
            ? immediateRequest
            : scheduleRequest;

        public override Visitor<Response> ResponseStructure => ResponseVisitor.Singleton;

        public enum Mode : byte
        {
            Immediately = 0,
            Schedule = 1
        }

        public enum Outcome : byte
        {
            BackgroundSavingStarted = 0,
            BackgroundSavingScheduled = 1,
            Error = 2
        }

        public readonly struct Response
        {
            public Response(Outcome outcome, string message)
            {
                Outcome = outcome;
                Message = message;
            }

            public Outcome Outcome { get; }
            public string Message { get; }
        }

        private sealed class ResponseVisitor : Expectation<Response>
        {
            private const string BackgroundSavingStarted = "Background saving started";
            private const string BackgroundSavingScheduled = "Background saving scheduled";
            public static ResponseVisitor Singleton { get; } = new ResponseVisitor();
            public override string Message => "SimpleString or Error";

            public override Response Visit(SimpleString simpleString)
            {
                return simpleString.Content switch
                {
                    BackgroundSavingStarted => new Response(
                        Outcome.BackgroundSavingStarted,
                        BackgroundSavingStarted
                    ),
                    BackgroundSavingScheduled => new Response(
                        Outcome.BackgroundSavingScheduled,
                        BackgroundSavingScheduled
                    ),
                    _ => throw Exception(simpleString)
                };
            }

            public override Response Visit(Error error) => new Response(Outcome.Error, error.Content);
        }
    }
}