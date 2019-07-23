using Rediska.Protocol;
using Rediska.Protocol.Visitors;

namespace Rediska.Commands.Strings
{
    public sealed class GET : Command<BulkString>
    {
        private static readonly PlainBulkString name = new PlainBulkString("GET");

        private readonly string key;

        public GET(string key)
        {
            this.key = key;
        }

        public override DataType Request => new PlainArray(
            name,
            new PlainBulkString(key)
        );

        public override Visitor<BulkString> ResponseStructure => BulkStringExpectation.Singleton;
    }
}