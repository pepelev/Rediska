namespace Rediska.Tests.Commands.PublishSubscribe
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Protocol;
    using Rediska.Commands.PublishSubscribe;

    public class Smoke
    {
        [Test]
        public async Task Test()
        {
            var connection = new SimpleConnection();
            var command = new PUBLISH("channel-1", new PlainBulkString("Hello"));
            var listened = await connection.ExecuteAsync(command).ConfigureAwait(false);
            Console.WriteLine(listened);
        }

        [Test]
        public async Task Listening()
        {
            var tcp = new TcpClient
            {
                NoDelay = true
            };
            await tcp.ConnectAsync(IPAddress.Loopback, 6379).ConfigureAwait(false);
            var stream = tcp.GetStream();
            var listening = new ListeningConnection(stream);
            var connection = new SimpleConnection(stream);
            var command = new SUBSCRIBE("channel-1");
            var noReply = new NoReplyConnection(new ConstResponse(new Integer(-1)), stream);
            await noReply.FireAndForgetAsync(command).ConfigureAwait(false);
            while (true)
            {
                var listenAsync = await listening.ListenAsync().ConfigureAwait(false);
            }
        }
    }
}