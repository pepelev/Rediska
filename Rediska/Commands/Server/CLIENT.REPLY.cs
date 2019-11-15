namespace Rediska.Commands.Server
{
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public static partial class CLIENT
    {
        public sealed class REPLY : Command<None>
        {
            private static readonly PlainBulkString subName = new PlainBulkString("REPLY");
            private readonly BulkString argument;

            public REPLY(BulkString argument)
            {
                this.argument = argument;
            }

            public static REPLY ON { get; } = new REPLY("ON");
            public static REPLY OFF { get; } = new REPLY("OFF");
            public static REPLY SKIP { get; } = new REPLY("SKIP");
            public override DataType Request => new PlainArray(name, subName, argument);
            public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
        }
    }
}