namespace Rediska.Commands.Scripting
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public static partial class SCRIPT
    {
        public sealed class LOAD : Command<Sha1>
        {
            private static readonly PlainBulkString subName = new PlainBulkString("LOAD");

            private static readonly Visitor<Sha1> responseStructure = BulkStringExpectation.Singleton
                .Then(bulkString => Sha1.Parse(bulkString.ToString()));

            private readonly string script;

            public LOAD(string script)
            {
                this.script = script;
            }

            public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
            {
                name,
                subName,
                factory.Utf8(script)
            };

            public override Visitor<Sha1> ResponseStructure => responseStructure;
        }
    }
}