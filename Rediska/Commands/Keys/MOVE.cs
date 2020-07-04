namespace Rediska.Commands.Keys
{
    using System;
    using Protocol;
    using Protocol.Visitors;

    public sealed class MOVE : Command<MOVE.Result>
    {
        public enum Result : byte
        {
            KeyMoved = 0,
            KeyNotMoved = 1,
        }

        private static readonly PlainBulkString name = new PlainBulkString("MOVE");
        private static readonly Visitor<Result> responseStructure = IntegerExpectation.Singleton.Then(Parse);
        private readonly Key key;
        private readonly DatabaseNumber destinationDb;

        public MOVE(Key key, DatabaseNumber destinationDb)
        {
            this.key = key;
            this.destinationDb = destinationDb;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString(),
            destinationDb.ToBulkString()
        );

        public override Visitor<Result> ResponseStructure => responseStructure;

        private static Result Parse(long response)
        {
            switch (response)
            {
                case 0:
                    return Result.KeyNotMoved;
                case 1:
                    return Result.KeyMoved;
                default:
                    throw new ArgumentOutOfRangeException(nameof(response), "Expected 0 or 1");
            }
        }
    }
}