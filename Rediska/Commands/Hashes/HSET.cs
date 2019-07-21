using Rediska.Protocol.Requests;
using Rediska.Protocol.Responses.Visitors;

namespace Rediska.Commands.Hashes
{
    public sealed class HSET : Command<HSET.Response>
    {
        public enum Response : byte
        {
            NewFieldAdded,
            OldFieldUpdated
        }

        private static readonly BulkString name = new BulkString("HSET");

        private readonly Key field;
        private readonly Key key;
        private readonly BulkString value;

        public HSET(Key key, Key field, BulkString value)
        {
            this.key = key;
            this.field = field;
            this.value = value;
        }

        public override DataType Request => new Array(
            name,
            key.ToBulkString(),
            field.ToBulkString(),
            value
        );

        public override Visitor<Response> ResponseStructure => new ProjectingVisitor<long, Response>(
            IntegerExpectation.Singleton,
            response => response == 0
                ? Response.OldFieldUpdated
                : Response.NewFieldAdded
        );
    }
}