namespace Rediska.Commands.Strings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Protocol;
    using Protocol.Visitors;

    public sealed class MSETNX : Command<MSETNX.Response>
    {
        public enum Response : byte
        {
            NotAllKeysSet = 0,
            AllKeysSet = 1
        }

        private static readonly PlainBulkString name = new PlainBulkString("MSETNX");
        private readonly IReadOnlyList<(Key Key, BulkString Value)> pairs;

        public MSETNX(params (Key Field, BulkString Value)[] pairs)
            : this(pairs as IReadOnlyList<(Key Field, BulkString Value)>)
        {
        }

        public MSETNX(IReadOnlyList<(Key Key, BulkString Value)> pairs)
        {
            if (pairs.Count < 1)
                throw new ArgumentException("Must contain at least one element", nameof(pairs));

            this.pairs = pairs;
        }

        public override DataType Request => new PlainArray(
            Query().ToList()
        );

        public override Visitor<Response> ResponseStructure => IntegerExpectation.Singleton.Then(
            response => response switch
            {
                0 => Response.NotAllKeysSet,
                1 => Response.AllKeysSet,
                _ => throw new ArgumentException($"Expected 0 or 1, but {response} found")
            }
        );

        public IEnumerable<BulkString> Query()
        {
            yield return name;
            foreach (var (key, value) in pairs)
            {
                yield return key.ToBulkString();
                yield return value;
            }
        }
    }
}