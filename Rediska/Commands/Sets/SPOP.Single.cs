using Rediska.Protocol.Requests;
using Rediska.Protocol.Responses.Visitors;
using BulkString = Rediska.Protocol.Responses.BulkString;
using DataType = Rediska.Protocol.Requests.DataType;

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

            public override DataType Request => new Array(
                name,
                key.ToBulkString()
            );

            public override Visitor<BulkString> ResponseStructure => BulkStringExpectation.Singleton;
        }
    }
}