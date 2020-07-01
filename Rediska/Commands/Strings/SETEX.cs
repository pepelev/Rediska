namespace Rediska.Commands.Strings
{
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class SETEX : Command<None>
    {
        private static readonly PlainBulkString name = new PlainBulkString("SETEX");
        private readonly Key key;
        private readonly long seconds;
        private readonly BulkString value;

        public SETEX(Key key, long seconds, BulkString value)
        {
            this.key = key;
            this.seconds = seconds;
            this.value = value;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString(),
            seconds.ToBulkString(),
            value
        );

        public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
    }
}