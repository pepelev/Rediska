using Rediska.Protocol;
using Rediska.Protocol.Visitors;

namespace Rediska.Commands.Lists
{
    public sealed class BLPOP : Command<Array>
    {
        private readonly Key key;

        public BLPOP(Key key)
        {
            this.key = key;
        }

        // todo
        public override DataType Request => new PlainArray(
            new PlainBulkString("BLPOP"),
            key.ToBulkString(),
            new PlainBulkString("1")
        );

        public override Visitor<Array> ResponseStructure => ArrayExpectation2.Singleton;
    }
}