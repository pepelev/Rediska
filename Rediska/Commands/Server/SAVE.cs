namespace Rediska.Commands.Server
{
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class SAVE : Command<None>
    {
        private static readonly PlainArray request = new PlainArray(new PlainBulkString("SAVE"));
        public override DataType Request => request;
        public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
    }
}