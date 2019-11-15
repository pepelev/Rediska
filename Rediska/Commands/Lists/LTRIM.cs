namespace Rediska.Commands.Lists
{
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class LTRIM : Command<None>
    {
        private static readonly PlainBulkString name = new PlainBulkString("LTRIM");
        private readonly Key key;
        private readonly Index start;
        private readonly Index stop;

        public LTRIM(Key key, Index start, Index stop)
        {
            this.key = key;
            this.start = start;
            this.stop = stop;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString(),
            start.ToBulkString(),
            stop.ToBulkString()
        );

        public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
    }
}