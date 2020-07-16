namespace Rediska.Commands.Connection
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class AUTH : Command<None>
    {
        private static readonly PlainBulkString name = new PlainBulkString("AUTH");
        private readonly string password;

        public AUTH(string password)
        {
            this.password = password;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            factory.Utf8(password)
        };

        public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
    }
}