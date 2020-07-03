namespace Rediska.Commands.Server
{
    using Protocol;
    using Protocol.Visitors;

    public sealed partial class COMMAND
    {
        public sealed class COUNT : Command<long>
        {
            private static readonly PlainBulkString subName = new PlainBulkString("COUNT");
            private static readonly PlainArray request = new PlainArray(name, subName);
            public static COUNT Singleton { get; } = new COUNT();
            public override DataType Request => request;
            public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;
        }
    }
}