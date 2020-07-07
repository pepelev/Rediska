namespace Rediska.Tests.Commands.SortedSets
{
    using System.Threading.Tasks;
    using Fixtures;
    using FluentAssertions;
    using NUnit.Framework;
    using Rediska.Commands.Keys;
    using Rediska.Commands.SortedSets;
    using static Rediska.Commands.SortedSets.ZADD.Mode;

    [TestFixtureSource(typeof(ConnectionCollection))]
    public class ZADD_Should
    {
        private readonly Connection connection;
        private Fixture fixture;

        public ZADD_Should(Connection connection)
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
        public async Task Do_Nothing_With_UpdateScoreOnly_Mode_When_Key_Does_Not_Exists()
        {
            var key = fixture.NewKey();

            await fixture.ExecuteAsync(
                new ZADD(key, UpdateScoreOnly, (10, "Member"))
            ).ConfigureAwait(false);

            var exists = await fixture.ExecuteAsync(
                new EXISTS(key)
            ).ConfigureAwait(false);
            exists.Should().Be(0);
        }
    }
}