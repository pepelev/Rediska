using Rediska.Protocol;
using Rediska.Protocol.Visitors;

namespace Rediska.Commands.Sets
{
    public static partial class SPOP
    {
        public sealed class Single : Command<BulkString>
        {
            private readonly Key key;

            public Single(Key key)
            {
                this.key = key;
            }

            public override DataType Request => new PlainArray(
                name,
                key.ToBulkString()
            );

            public override Visitor<BulkString> ResponseStructure => BulkStringExpectation.Singleton;
        }
    }
}