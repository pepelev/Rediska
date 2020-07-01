namespace Rediska.Commands.Strings
{
    using System;
    using Protocol;
    using Protocol.Visitors;

    public sealed class SETBIT : Command<SETBIT.Response>
    {
        private static readonly PlainBulkString name = new PlainBulkString("SETBIT");

        private static readonly Visitor<Response> responseStructure = IntegerExpectation.Singleton
            .Then(
                bitValueBeforeOperation => bitValueBeforeOperation switch
                {
                    0 => new Response(false),
                    1 => new Response(true),
                    _ => throw new ArgumentException($"Expected 0 or 1, but was {bitValueBeforeOperation}")
                }
            );

        private readonly Key key;
        private readonly uint offset;
        private readonly bool value;

        public SETBIT(Key key, uint offset, bool value)
        {
            this.key = key;
            this.offset = offset;
            this.value = value;
        }

        private BulkString Value => value
            ? BulkStringConstants.One
            : BulkStringConstants.Zero;

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString(),
            offset.ToBulkString(),
            Value
        );

        public override Visitor<Response> ResponseStructure => responseStructure;

        public readonly struct Response
        {
            public Response(bool bitValueBeforeOperation)
            {
                BitValueBeforeOperation = bitValueBeforeOperation;
            }

            public bool BitValueBeforeOperation { get; }
        }
    }
}