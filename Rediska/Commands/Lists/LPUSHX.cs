namespace Rediska.Commands.Lists
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class LPUSHX : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("LPUSHX");
        private readonly Key key;
        private readonly IReadOnlyList<BulkString> values;

        public LPUSHX(Key key, params BulkString[] values)
            : this(key, values as IReadOnlyList<BulkString>)
        {
        }

        public LPUSHX(Key key, IReadOnlyList<BulkString> values)
        {
            this.key = key;
            this.values = values;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new ConcatList<BulkString>(
            new[]
            {
                name,
                key.ToBulkString(factory)
            },
            values
        );

        public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;
    }
}