namespace Rediska.Commands.Strings
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class SETNX : Command<SETNX.Response>
    {
        private static readonly PlainBulkString name = new PlainBulkString("SETNX");
        private readonly Key key;
        private readonly BulkString value;

        public SETNX(Key key, BulkString value)
        {
            this.key = key;
            this.value = value;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory),
            value
        };

        public override Visitor<Response> ResponseStructure => IntegerExpectation.Singleton.Then(
            response => response switch
            {
                0 => Response.KeyWasNotSet,
                1 => Response.KeyWasSet,
                _ => throw new ArgumentException($"Expected 0 or 1, but found {response}")
            }
        );

        public enum Response : byte
        {
            KeyWasNotSet = 0,
            KeyWasSet = 1
        }
    }
}