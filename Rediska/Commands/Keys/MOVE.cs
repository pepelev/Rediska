namespace Rediska.Commands.Keys
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class MOVE : Command<MOVE.Result>
    {
        private static readonly PlainBulkString name = new PlainBulkString("MOVE");
        private static readonly Visitor<Result> responseStructure = IntegerExpectation.Singleton.Then(Parse);
        private readonly Key key;
        private readonly DatabaseNumber destinationDb;

        public MOVE(Key key, DatabaseNumber destinationDb)
        {
            this.key = key;
            this.destinationDb = destinationDb;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory),
            factory.Create(destinationDb.Value)
        };

        public override Visitor<Result> ResponseStructure => responseStructure;

        private static Result Parse(long response)
        {
            return response switch
            {
                0 => Result.KeyNotMoved,
                1 => Result.KeyMoved,
                _ => throw new ArgumentOutOfRangeException(nameof(response), "Must be either 0 or 1")
            };
        }

        public enum Result : byte
        {
            KeyMoved = 0,
            KeyNotMoved = 1,
        }
    }
}