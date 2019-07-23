using Rediska.Protocol.Responses;
using Rediska.Protocol.Responses.Visitors;
using Array = Rediska.Protocol.Requests.Array;
using BulkString = Rediska.Protocol.Requests.BulkString;
using DataType = Rediska.Protocol.Requests.DataType;

namespace Rediska.Commands.Strings
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

        // todo
        public override Visitor<Protocol.Responses.BulkString> ResponseStructure => new ConstVisitor<Protocol.Responses.BulkString>(
            new PlainBulkString(new byte[0])
        );
    }
}