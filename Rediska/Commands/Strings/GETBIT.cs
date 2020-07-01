namespace Rediska.Commands.Strings
{
    using System;
    using Protocol;
    using Protocol.Visitors;

    public sealed class GETBIT : Command<bool>
    {
        private static readonly PlainBulkString name = new PlainBulkString("GETBIT");

        private static readonly Visitor<bool> responseStructure = IntegerExpectation.Singleton
            .Then(
                response => response switch
                {
                    0 => false,
                    1 => true,
                    var value => throw new ArgumentException($"Expected 0 or 1, but found {value}")
                }
            );

        private readonly Key key;
        private readonly uint offset;

        public GETBIT(Key key, uint offset)
        {
            this.key = key;
            this.offset = offset;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString(),
            offset.ToBulkString()
        );

        public override Visitor<bool> ResponseStructure => responseStructure;
    }
}