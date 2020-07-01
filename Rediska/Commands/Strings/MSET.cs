namespace Rediska.Commands.Strings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class MSET : Command<None>
    {
        private static readonly PlainBulkString name = new PlainBulkString("MSET");
        private readonly IReadOnlyList<(Key Key, BulkString Value)> pairs;

        public MSET(params (Key Field, BulkString Value)[] pairs)
            : this(pairs as IReadOnlyList<(Key Field, BulkString Value)>)
        {
        }

        public MSET(IReadOnlyList<(Key Key, BulkString Value)> pairs)
        {
            if (pairs.Count < 1)
                throw new ArgumentException("Must contain at least one element", nameof(pairs));

            this.pairs = pairs;
        }

        public override DataType Request => new PlainArray(
            Query().ToList()
        );

        public override Visitor<None> ResponseStructure => OkExpectation.Singleton;

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