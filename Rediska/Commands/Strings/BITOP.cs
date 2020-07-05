namespace Rediska.Commands.Strings
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class BITOP : Command<BITOP.Response>
    {
        private static readonly PlainBulkString name = new PlainBulkString("BITOP");
        private static readonly PlainBulkString and = new PlainBulkString("AND");
        private static readonly PlainBulkString or = new PlainBulkString("OR");
        private static readonly PlainBulkString xor = new PlainBulkString("XOR");
        private static readonly PlainBulkString not = new PlainBulkString("NOT");
        private static readonly Visitor<Response> responseStructure = IntegerExpectation.Singleton.Then(destinationStringLength => new Response(destinationStringLength));
        private readonly BinaryOperation operation;
        private readonly Key destination;
        private readonly IReadOnlyList<Key> keys;

        public BITOP(BinaryOperation operation, Key destination, params Key[] keys)
            : this(operation, destination, keys as IReadOnlyList<Key>)
        {
        }

        public BITOP(BinaryOperation operation, Key destination, IReadOnlyList<Key> keys)
        {
            if (operation != BinaryOperation.And &&
                operation != BinaryOperation.Or &&
                operation != BinaryOperation.Xor)
            {
                throw new ArgumentException("Must be And, Or or Xor", nameof(operation));
            }

            this.operation = operation;
            this.destination = destination;
            this.keys = keys;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new ConcatList<BulkString>(
            new[]
            {
                name,
                Operation,
                destination.ToBulkString(factory)
            },
            new KeyList(factory, keys)
        );

        public override Visitor<Response> ResponseStructure => responseStructure;

        private BulkString Operation => operation switch
        {
            BinaryOperation.And => and,
            BinaryOperation.Or => or,
            BinaryOperation.Xor => xor,
            _ => throw new Exception("There is a bug in constructor")
        };

        public readonly struct Response
        {
            public Response(long destinationStringLength)
            {
                DestinationStringLength = destinationStringLength;
            }

            public long DestinationStringLength { get; }
        }

        public sealed class NOT : Command<Response>
        {
            private readonly Key destination;
            private readonly Key argument;

            public NOT(Key destination, Key argument)
            {
                this.destination = destination;
                this.argument = argument;
            }

            public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
            {
                name,
                not,
                destination.ToBulkString(factory),
                argument.ToBulkString(factory)
            };

            public override Visitor<Response> ResponseStructure => responseStructure;
        }
    }
}