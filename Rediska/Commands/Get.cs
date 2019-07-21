using Array = Rediska.Tests.Protocol.Requests.Array;
using BulkString = Rediska.Tests.Protocol.Requests.BulkString;
using DataType = Rediska.Tests.Protocol.Requests.DataType;

namespace Rediska.Commands
{
    public sealed class GET : Command<Protocol.Responses.BulkString>
    {
        private readonly string key;

        public GET(string key)
        {
            this.key = key;
        }

        public override DataType Request => new Array(
            new BulkString("GET"),
            new BulkString(key)
        );

        public override Visitor<Protocol.Responses.BulkString> ResponseStructure => new ConstVisitor<Protocol.Responses.BulkString>(new PlainBulkString(new byte[0]));
    }
}