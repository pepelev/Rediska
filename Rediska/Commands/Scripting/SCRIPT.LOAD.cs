namespace Rediska.Commands.Scripting
{
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

            public override DataType Request => new PlainArray(
                name,
                subName,
                new PlainBulkString(script)
            );

            public override Visitor<Sha1> ResponseStructure => responseStructure;
        }
    }
}