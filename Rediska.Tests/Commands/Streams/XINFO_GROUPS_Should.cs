namespace Rediska.Tests.Commands.Streams
{
    using System.Linq;
    using System.Threading.Tasks;
    using Fixtures;
    using FluentAssertions;
    using NUnit.Framework;
    using Rediska.Commands.Streams;
    using static Rediska.Commands.Streams.XGROUP.CREATE.Mode;

    [TestFixtureSource(typeof(ConnectionCollection))]
    public sealed class XINFO_GROUPS_Should
    {
        private readonly Connection connection;
        private Fixture fixture;

        public XINFO_GROUPS_Should(Connection connection)
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
        public async Task Give_Info_For_Empty_Stream()
        {
            var key = fixture.NewKey();
            var xgroupCreate = new XGROUP.CREATE(key, "group", Offset.EndOfStream, CreateStreamIfNotExists);
            await fixture.ExecuteAsync(xgroupCreate).ConfigureAwait(false);

            var sut = new XINFO.GROUPS(key);
            var groups = await fixture.ExecuteAsync(sut).ConfigureAwait(false);

            groups.Select(
                group => new
                {
                    group.Name,
                    group.Consumers,
                    group.PendingEntries,
                    group.LastDelivered
                }
            ).Should().Equal(
                new
                {
                    Name = "group",
                    Consumers = 0L,
                    PendingEntries = 0L,
                    LastDelivered = Id.Minimum
                }
            );
        }
    }
}