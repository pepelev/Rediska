namespace Rediska.Commands.Connection
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public static partial class CLIENT
    {
        public sealed class UNBLOCK : Command<UNBLOCK.Response>
        {
            private static readonly PlainBulkString subName = new PlainBulkString("UNBLOCK");
            private static readonly PlainBulkString error = new PlainBulkString("ERROR");

            private static readonly Visitor<Response> responseStructure = IntegerExpectation.Singleton
                .Then(reply => new Response(reply));

            private readonly ClientId clientId;
            private readonly Mode mode;

            public UNBLOCK(ClientId clientId, Mode mode)
            {
                if (mode != Mode.Timeout && mode != Mode.Error)
                    throw new ArgumentException("Must be either Timeout or Error", nameof(mode));

                this.clientId = clientId;
                this.mode = mode;
            }

            public override IEnumerable<BulkString> Request(BulkStringFactory factory) => mode == Mode.Timeout
                ? new[]
                {
                    name,
                    subName,
                    clientId.ToBulkString(factory)
                }
                : new[]
                {
                    name,
                    subName,
                    clientId.ToBulkString(factory),
                    error
                };

            public override Visitor<Response> ResponseStructure => responseStructure;

            public enum Mode : byte
            {
                Timeout = 0,
                Error = 1
            }

            public readonly struct Response
            {
                public Response(long reply)
                {
                    Reply = reply;
                }

                public long Reply { get; }
                public bool ClientUnblocked => Reply == 1;

                public override string ToString() => ClientUnblocked
                    ? "Client unblocked"
                    : "Client not unblocked";
            }
        }
    }
}