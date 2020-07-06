using Rediska.Protocol;
using Rediska.Protocol.Visitors;

namespace Rediska.Commands.Hashes
{
    using System.Collections.Generic;

    public sealed class HEXISTS : Command<HEXISTS.Response>
    {
        public enum Response
        {
            FieldExists,
            KeyOrFieldNotExists
        }

        private static readonly PlainBulkString name = new PlainBulkString("HEXISTS");

        private static readonly Visitor<Response> responseStructure = IntegerExpectation.Singleton
            .Then(
                response => response == 1
                    ? Response.FieldExists
                    : Response.KeyOrFieldNotExists
            );

        private readonly Key key;
        private readonly BulkString field;

        public HEXISTS(Key key, BulkString field)
        {
            this.key = key;
            this.field = field;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory),
            field
        };

        public override Visitor<Response> ResponseStructure => responseStructure;
    }
}