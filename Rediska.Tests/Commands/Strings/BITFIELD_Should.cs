namespace Rediska.Tests.Commands.Strings
{
    using System.Threading.Tasks;
    using Fixtures;
    using FluentAssertions;
    using NUnit.Framework;
    using Rediska.Commands.Strings;
    using static Rediska.Commands.Strings.Offset;
    using static Rediska.Commands.Strings.Type;

    // todo show what redis returns for zero operations 
    // todo show what redis returns for zero size
    [TestFixtureSource(typeof(ConnectionCollection))]
    public sealed class BITFIELD_Should
    {
        private readonly Connection connection;
        private Fixture fixture;

        public BITFIELD_Should(Connection connection)
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
        public async Task Return_Zero_On_Set_When_Key_Does_Not_Exists()
        {
            var key = fixture.NewKey();
            var command = new BITFIELD(
                key,
                new BITFIELD.SET(Unsigned(32), Zero, 2_000_000_000)
            );
            var result = await fixture.ExecuteAsync(command).ConfigureAwait(false);

            result.Should().Equal(0L);
        }

        [Test]
        public async Task Zero_Sized_Type()
        {
            var sut = new BITFIELD(
                "hello",
                new BITFIELD.INCRBY(Signed(1), Zero, -9, Overflow.Fail)
            );
            var result = await connection.ExecuteAsync(sut).ConfigureAwait(false);
        }
    }
}