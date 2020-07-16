namespace Rediska.Commands.Connection
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public static partial class CLIENT
    {
        public sealed class ID : Command<ClientId>
        {
            private static readonly PlainBulkString subName = new PlainBulkString("ID");
            private static readonly PlainBulkString[] request = {name, subName};

            private static readonly Visitor<ClientId> responseStructure = IntegerExpectation.Singleton
                .Then(clientId => new ClientId(clientId));

            public override IEnumerable<BulkString> Request(BulkStringFactory factory) => request;
            public override Visitor<ClientId> ResponseStructure => responseStructure;
        }
    }
}