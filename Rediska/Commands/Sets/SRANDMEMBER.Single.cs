namespace Rediska.Commands.Sets
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public static partial class SRANDMEMBER
    {
        public sealed class Single : Command<BulkString>
        {
            private readonly Key key;

            public Single(Key key)
            {
                this.key = key;
            }

            public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
            {
                name,
                key.ToBulkString(factory)
            };

            public override Visitor<BulkString> ResponseStructure => BulkStringExpectation.Singleton;
        }
    }
}