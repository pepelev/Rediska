namespace Rediska.Commands.PublishSubscribe
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class PUBLISH : Command<PUBLISH.Response>
    {
        private static readonly PlainBulkString name = new PlainBulkString("PUBLISH");

        private static readonly Visitor<Response> responseStructure = IntegerExpectation.Singleton
            .Then(response => new Response(response));

        private readonly Channel channel;
        private readonly BulkString message;

        public PUBLISH(Channel channel, BulkString message)
        {
            this.channel = channel;
            this.message = message;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            factory.Utf8(channel.Name),
            message
        };

        public override Visitor<Response> ResponseStructure => responseStructure;

        public readonly struct Response
        {
            public Response(long numberOfClientsThatReceiveMessage)
            {
                NumberOfClientsThatReceiveMessage = numberOfClientsThatReceiveMessage;
            }

            public long NumberOfClientsThatReceiveMessage { get; }

            public override string ToString() =>
                $"{nameof(NumberOfClientsThatReceiveMessage)}: {NumberOfClientsThatReceiveMessage}";
        }
    }
}