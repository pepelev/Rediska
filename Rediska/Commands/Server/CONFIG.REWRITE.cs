namespace Rediska.Commands.Server
{
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public static partial class CONFIG
    {
        public sealed class REWRITE : Command<None>
        {
            private static readonly PlainBulkString subName = new PlainBulkString("REWRITE");

            private static readonly PlainArray request = new PlainArray(
                name,
                subName
            );

            public override DataType Request => request;
            public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
        }
    }
}