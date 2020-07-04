namespace Rediska.Commands.Scripting
{
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public static partial class SCRIPT
    {
        public sealed class KILL : Command<None>
        {
            private static readonly PlainBulkString subName = new PlainBulkString("KILL");
            private static readonly PlainArray request = new PlainArray(name, subName);
            public override DataType Request => request;
            public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
            public static KILL Singleton { get; } = new KILL();
        }
    }
}