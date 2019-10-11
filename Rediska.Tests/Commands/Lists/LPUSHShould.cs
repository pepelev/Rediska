namespace Rediska.Tests.Commands.Lists
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using NUnit.Framework;
    using Protocol;
    using Rediska.Commands.Lists;
    using Sets;
    using Utils;

    [TestFixtureSource(typeof(LocalRedis))]
    public sealed class LPUSHShould
    {
        private readonly ConnectionFixture fixture;
        private Resource<Connection>? connection;

        public LPUSHShould(ConnectionFixture fixture)
        {
            this.fixture = fixture;
        }

        public Connection Connection => connection?.Value ?? throw new InvalidOperationException("connection is null");

        [SetUp]
        public async Task SetUpAsync()
        {
            connection = new Resource<Connection>(
                await fixture.SetUpAsync().ConfigureAwait(false)
            );
        }

        [Test]
        public async Task Test()
        {
            var command = new LPUSH("dosya", new PlainBulkString("12313"));
            var count = await Connection.ExecuteAsync(command).ConfigureAwait(false);
            count.Should().Be(1);
        }

        [TearDown]
        public async Task TearDownAsync()
        {
            connection?.Dispose();
            await fixture.TearDownAsync().ConfigureAwait(false);
        }
    }
}