namespace Rediska.Commands.Strings
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class STRLEN : Command<Size>
    {
        private static readonly PlainBulkString name = new PlainBulkString("STRLEN");

        private static readonly Visitor<Size> responseStructure = IntegerExpectation.Singleton
            .Then(
                length => length >= 0
                    ? new Size((ulong) length)
                    : throw new ArgumentOutOfRangeException(nameof(length), length, "Must be non-negative")
            );

        private readonly Key key;

        public STRLEN(Key key)
        {
            this.key = key;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory)
        };

        public override Visitor<Size> ResponseStructure => responseStructure;
    }
}