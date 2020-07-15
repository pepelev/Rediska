using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Rediska.Commands;

namespace Rediska.Tests.Tests
{
    using System.Linq;
    using System.Threading;
    using Protocol;
    using Rediska.Commands.Connection;

    public sealed class ConnectionShould
    {
        private SimpleConnection sut;

        [SetUp]
        public void SetUp()
        {
            sut = new SimpleConnection();
        }

        [Test]
        public async Task Test()
        {
            var command = new ECHO("foo");
            var request = new PlainArray(
                command.Request(BulkStringFactory.Plain).ToList()
            );
            var response = await sut.SendAsync(request, CancellationToken.None).ConfigureAwait(false);
            var dataType = await response.ReadAsync().ConfigureAwait(false);
            var echo = dataType.Accept(command.ResponseStructure);
            echo.Should().Be("foo");
        }
    }
}