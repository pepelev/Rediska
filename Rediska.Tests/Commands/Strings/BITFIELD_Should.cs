namespace Rediska.Tests.Commands.Strings
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using NUnit.Framework;
    using Rediska.Commands.Strings;
    using Sets;
    using static Rediska.Commands.Strings.BITFIELD;
    using static Rediska.Commands.Strings.Offset;
    using static Rediska.Commands.Strings.Type;

    [TestFixtureSource(typeof(ConnectionCollection))]
    public sealed class BITFIELD_Should
    {
        private readonly Connection connection;

        public BITFIELD_Should(Connection connection)
        {
            this.connection = connection;
        }

        [Test]
        public async Task Return_What_Set()
        {
            var result = await connection.ExecuteAsync(
                new BITFIELD(
                    "hello",
                    new SET(Unsigned(32), Zero, 2_000_000_000)
                )
            ).ConfigureAwait(false);

            result.Should().Equal(2_000_000_000);
        }

        [Test]
        public async Task METHOD()
        {
            var sut = new BITFIELD(
                "hello",
                new INCRBY(Signed(32), Zero, 0, Overflow.Saturate),
                new SET(Signed(32), Zero, 4_000_000_000)
            );
            var result = await connection.ExecuteAsync(sut).ConfigureAwait(false);

            result.Should().Equal(2_000_000_000);
        }

        [Test]
        public async Task Zero_Sized_Type()
        {
            var sut = new BITFIELD(
                "hello",
                new INCRBY(Signed(1), Zero, -9, Overflow.Fail)
            );
            var result = await connection.ExecuteAsync(sut).ConfigureAwait(false);
        }
    }
}