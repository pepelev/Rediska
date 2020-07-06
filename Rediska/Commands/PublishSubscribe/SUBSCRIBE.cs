namespace Rediska.Commands.PublishSubscribe
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class SUBSCRIBE : Command<None>
    {
        private static readonly PlainBulkString name = new PlainBulkString("SUBSCRIBE");
        private readonly IReadOnlyList<Channel> channels;

        public SUBSCRIBE(params Channel[] channels)
            : this(channels as IReadOnlyList<Channel>)
        {
        }

        public SUBSCRIBE(IReadOnlyList<Channel> channels)
        {
            this.channels = channels;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory)
        {
            return new PrefixedList<BulkString>(
                name,
                new ProjectingReadOnlyList<Channel, BulkString>(
                    channels,
                    channel => factory.Utf8(channel.Name)
                )
            );
        }

        // todo parse
        public override Visitor<None> ResponseStructure => ArrayExpectation.Singleton.Then(_ => new None());
    }
}