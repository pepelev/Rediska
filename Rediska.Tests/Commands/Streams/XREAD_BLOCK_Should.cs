namespace Rediska.Tests.Commands.Streams
{
    using System.Threading.Tasks;
    using Fixtures;
    using FluentAssertions;
    using NUnit.Framework;
    using Rediska.Commands;
    using Rediska.Commands.Streams;

    [TestFixtureSource(typeof(ConnectionCollection))]
    public sealed class XREAD_BLOCK_Should
    {
        private readonly Connection connection;
        private Fixture fixture;

        public XREAD_BLOCK_Should(Connection connection)
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
        public async Task Return_Timeout_When_No_Additions_Occured()
        {
            var key = fixture.NewKey();
            var xadd = new XADD(key, ("Field", "Value"));
            await fixture.ExecuteAsync(xadd).ConfigureAwait(false);

            var sut = new XREAD.BLOCK(Count.Unbound, new MillisecondsTimeout(10), (key, Offset.EndOfStream));
            var response = await fixture.ExecuteAsync(sut).ConfigureAwait(false);

            response.Outcome.Should().Be(XREAD.BLOCK.Outcome.Timeout);
        }
    }
}