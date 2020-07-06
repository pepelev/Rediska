namespace Rediska.Commands.Keys
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class RANDOMKEY : Command<Key>
    {
        private static readonly PlainBulkString name = new PlainBulkString("RANDOMKEY");
        private static readonly BulkString[] request = {name};
        private static readonly Visitor<Key> responseStructure = BulkStringExpectation.Singleton.Then(Parse);
        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => request;
        public override Visitor<Key> ResponseStructure => responseStructure;

        private static Key Parse(BulkString response)
        {
            return response.IsNull
                ? null
                : new Key.BulkString(response);
        }
    }
}