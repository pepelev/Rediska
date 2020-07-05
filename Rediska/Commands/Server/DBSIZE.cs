namespace Rediska.Commands.Server
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class DBSIZE : Command<DBSIZE.Response>
    {
        private static readonly BulkString[] request = {new PlainBulkString("DBSIZE")};

        private static readonly Visitor<Response> responseStructure = IntegerExpectation.Singleton
            .Then(numberOfKeysInDatabase => new Response(numberOfKeysInDatabase));

        public static DBSIZE Singleton { get; } = new DBSIZE();
        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => request;
        public override Visitor<Response> ResponseStructure => responseStructure;

        public readonly struct Response
        {
            public Response(long numberOfKeysInDatabase)
            {
                if (numberOfKeysInDatabase < 0)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(numberOfKeysInDatabase),
                        numberOfKeysInDatabase,
                        "Must be non-negative"
                    );
                }

                NumberOfKeysInDatabase = numberOfKeysInDatabase;
            }

            public long NumberOfKeysInDatabase { get; }
            public override string ToString() => $"{nameof(NumberOfKeysInDatabase)}: {NumberOfKeysInDatabase}";
        }
    }
}