namespace Rediska.Commands.Server
{
    using Protocol;
    using Protocol.Visitors;

    public sealed class DBSIZE : Command<long>
    {
        private static readonly Array request = new PlainArray(
            new PlainBulkString("DBSIZE")
        );

        public static DBSIZE Singleton { get; } = new DBSIZE();
        public override DataType Request => request;
        public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;
    }
}