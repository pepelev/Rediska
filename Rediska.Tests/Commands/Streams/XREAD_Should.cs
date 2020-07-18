namespace Rediska.Tests.Commands.Streams
{
    using System.Threading.Tasks;
    using Fixtures;
    using FluentAssertions;
    using NUnit.Framework;
    using Rediska.Commands.Streams;

    [TestFixtureSource(typeof(ConnectionCollection))]
    public sealed class XREAD_Should
    {
        private readonly Connection connection;
        private Fixture fixture;

        public XREAD_Should(Connection connection)
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
        public async Task Read_Non_Existing_Stream()
        {
            var key = fixture.NewKey();
            var sut = new XREAD(Count.Unbound, (key, Id.Minimum));
            var response = await fixture.ExecuteAsync(sut).ConfigureAwait(false);

            response.Should().BeEmpty();
        }

        [Test]
        public async Task Read_Added_Entry()
        {
            var key = fixture.NewKey();
            var xadd = new XADD(key, ("Field", "Value"));
            var addResponse = await fixture.ExecuteAsync(xadd).ConfigureAwait(false);

            var sut = new XREAD(Count.Unbound, (key, Id.Minimum));
            var response = await fixture.ExecuteAsync(sut).ConfigureAwait(false);

            response.Should().HaveCount(1);
            var entries = response[0];
            entries.Stream.ToBytes().Should().Equal(key.ToBytes());
            entries.Should().HaveCount(1);
            entries[0].Id.Should().Be(addResponse.AddedEntryId);
            entries[0].Should().Equal(("Field", "Value"));
        }
    }
}