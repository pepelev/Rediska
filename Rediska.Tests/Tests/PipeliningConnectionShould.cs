namespace Rediska.Tests.Tests
{
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using NUnit.Framework;
    using Protocol;
    using Rediska.Commands;
    using Rediska.Commands.Connection;

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

            var firstResponse = await sut.SendAsync(
                new PlainArray(
                    firstCommand.Request(BulkStringFactory.Plain).ToList()
                ),
                CancellationToken.None
            ).ConfigureAwait(false);
            var secondResponse = await sut.SendAsync(
                new PlainArray(
                    secondCommand.Request(BulkStringFactory.Plain).ToList()
                ),
                CancellationToken.None
            ).ConfigureAwait(false);
            var second = await secondResponse.ReadAsync().ConfigureAwait(false);
            var first = await firstResponse.ReadAsync().ConfigureAwait(false);
            second.Accept(secondCommand.ResponseStructure).Should().Be("bar");
            first.Accept(firstCommand.ResponseStructure).Should().Be("foo");
        }
    }
}