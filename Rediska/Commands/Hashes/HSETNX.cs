namespace Rediska.Commands.Hashes
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class HSETNX : Command<HSETNX.Response>
    {
        private static readonly PlainBulkString name = new PlainBulkString("HSETNX");
        private readonly Key key;
        private readonly BulkString field;
        private readonly BulkString value;

        public HSETNX(Key key, BulkString field, BulkString value)
        {
            this.key = key;
            this.field = field;
            this.value = value;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory),
            field,
            value
        };

        public override Visitor<Response> ResponseStructure => new ProjectingVisitor<long, Response>(
            IntegerExpectation.Singleton,
            response => response switch
            {
                0 => Response.FieldAlreadyExists,
                1 => Response.NewFieldAdded,
                _ => throw new ArgumentOutOfRangeException(nameof(response), response, "Expected 0 or 1")
            }
        );

        public enum Response : byte
        {
            NewFieldAdded = 0,
            FieldAlreadyExists = 1
        }
    }
}