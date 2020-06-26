namespace Rediska.Commands.Keys
{
    using System;
    using Protocol;
    using Protocol.Visitors;

    public sealed class PERSIST : Command<PERSIST.Result>
    {
        public enum Result : byte
        {
            TimeoutNotRemoved,
            TimeoutRemoved
        }

        private static readonly PlainBulkString name = new PlainBulkString("PERSIST");
        private static readonly Visitor<Result> responseStructure = IntegerExpectation.Singleton.Then(Parse);
        private readonly Key key;

        public PERSIST(Key key)
        {
            this.key = key;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString()
        );

        public override Visitor<Result> ResponseStructure => responseStructure;

        private static Result Parse(long response)
        {
            switch (response)
            {
                case 0:
                    return Result.TimeoutNotRemoved;
                case 1:
                    return Result.TimeoutRemoved;
                default:
                    throw new ArgumentOutOfRangeException(nameof(response), "Expected 0 or 1");
            }
        }
    }
}