namespace Rediska.Commands.Server
{
    using System;
    using Protocol;
    using Protocol.Visitors;
    using Array = Protocol.Array;

    public sealed class DBSIZE : Command<DBSIZE.Response>
    {
        private static readonly Array request = new PlainArray(
            new PlainBulkString("DBSIZE")
        );

        private static readonly Visitor<Response> responseStructure = IntegerExpectation.Singleton
            .Then(numberOfKeysInDatabase => new Response(numberOfKeysInDatabase));

        public static DBSIZE Singleton { get; } = new DBSIZE();
        public override DataType Request => request;
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
        }
    }
}