namespace Rediska.Commands.Keys
{
    using Protocol;
    using Protocol.Visitors;

    public sealed class EXPIRE : Command<ExpireResult>
    {
        private static readonly PlainBulkString name = new PlainBulkString("EXPIRE");
        private readonly Key key;
        private readonly long seconds;

        public EXPIRE(Key key, long seconds)
        {
            this.key = key;
            this.seconds = seconds;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString(),
            seconds.ToBulkString()
        );

        public override Visitor<ExpireResult> ResponseStructure => CompositeVisitors.ExpireResult;
    }
}