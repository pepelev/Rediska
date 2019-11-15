namespace Rediska.Commands.PublishSubscribe
{
    using Protocol;
    using Protocol.Visitors;

    public sealed class PUBLISH : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("PUBLISH");

        private readonly Channel channel;
        private readonly BulkString message;

        public PUBLISH(Channel channel, BulkString message)
        {
            this.channel = channel;
            this.message = message;
        }

        public override DataType Request => new PlainArray(
            name,
            new PlainBulkString(channel.ToBytes()),
            message
        );

        public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;
    }
}