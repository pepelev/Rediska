namespace Rediska.Tests.Commands.Lists
{
    using System.Threading.Tasks;
    using Fixtures;
    using FluentAssertions;
    using NUnit.Framework;
    using Protocol;
    using Rediska.Commands.Lists;

    [TestFixtureSource(typeof(ConnectionCollection))]
    public sealed class LPUSH_Should
    {
        private readonly Connection connection;
        private Fixture fixture;

        public LPUSH_Should(Connection connection)
        {
            this.connection = connection;
        }

        [SetUp]
        public void SetUp()
        {
            fixture = new Fixture(connection);
        }

        [TearDown]
        public async Task TearDownAsync()
        {
            await fixture.TearDownAsync().ConfigureAwait(false);
        }

        [Test]
        public async Task Return_Count_Of_What()
        {
            var key = fixture.NewKey();
            var command = new LPUSH(key, new PlainBulkString("12313"));
            var count = await fixture.ExecuteAsync(command).ConfigureAwait(false);
            count.Should().Be(1);
        }
    }
}