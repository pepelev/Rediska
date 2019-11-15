namespace Rediska.Tests.Tests
{
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using FluentAssertions;
    using NUnit.Framework;
    using Rediska.Commands;

    public sealed class PipeliningConnectionShould
    {
        private PipeliningConnection sut;

        [SetUp]
        public void SetUp()
        {
            var tcp = new TcpClient
            {
                NoDelay = true
            };
            tcp.Connect(IPAddress.Loopback, 6379);
            var stream = tcp.GetStream();
            sut = new PipeliningConnection(stream);
        }

        [Test]
        public async Task Test()
        {
            var command = new ECHO("foo");
            var echo = await sut.ExecuteAsync(command).ConfigureAwait(false);
            echo.Should().Be("foo");
        }

        [Test]
        public async Task Two_Requests()
        {
            var firstCommand = new ECHO("foo");
            var secondCommand = new ECHO("bar");

            using (var firstResponse = await sut.SendAsync(firstCommand.Request).ConfigureAwait(false))
            using (var secondResponse = await sut.SendAsync(secondCommand.Request).ConfigureAwait(false))
            {
                var second = await secondResponse.Value.ReadAsync().ConfigureAwait(false);
                var first = await firstResponse.Value.ReadAsync().ConfigureAwait(false);
                second.Accept(secondCommand.ResponseStructure).Should().Be("bar");
                first.Accept(firstCommand.ResponseStructure).Should().Be("foo");
            }
        }
    }
}