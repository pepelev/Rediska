namespace Rediska.Commands.Sets
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class SADD : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("SADD");
        private readonly Key key;
        private readonly IReadOnlyList<BulkString> members;

        public SADD(Key key, params BulkString[] members)
            : this(key, members as IReadOnlyList<BulkString>)
        {
        }

        public SADD(Key key, IReadOnlyList<BulkString> members)
        {
            this.key = key;
            this.members = members;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new ConcatList<BulkString>(
            new[]
            {
                name,
                key.ToBulkString(factory)
            },
            members
        );

        public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;
    }
}