namespace Rediska.Commands.Lists
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class RPUSHX : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("RPUSHX");
        private readonly Key key;
        private readonly IReadOnlyList<BulkString> values;

        public RPUSHX(Key key, params BulkString[] values)
            : this(key, values as IReadOnlyList<BulkString>)
        {
        }

        public RPUSHX(Key key, IReadOnlyList<BulkString> values)
        {
            this.key = key;
            this.values = values;
        }

        public override DataType Request => new PlainArray(
            new ConcatList<DataType>(
                new DataType[]
                {
                    name,
                    key.ToBulkString()
                },
                values
            )
        );

        public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;
    }
}