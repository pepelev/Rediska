namespace Rediska.Tests.Commands.Lists
{
    using System;
    using System.Threading.Tasks;
    using Fixtures;
    using FluentAssertions;
    using NUnit.Framework;
    using Rediska.Commands.Lists;
    using static Rediska.Commands.Lists.LINSERT.Where;

    [TestFixtureSource(typeof(ConnectionCollection))]
    public sealed class LINSERT_Should
    {
        private readonly Connection connection;
        private Fixture fixture;

        public LINSERT_Should(Connection connection)
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
        public async Task Reply_With_Zero_Against_Empty_List([Values(BEFORE, AFTER)] LINSERT.Where where)
        {
            var sut = new LINSERT("non-existent-key", where, "Pivot", "NewElement");

            var result = await fixture.ExecuteAsync(sut).ConfigureAwait(false);

            result.Reply.Should().Be(0);
        }

        [Test]
        public async Task Reply_MinusOne_Against_List_Without_Pivot([Values(BEFORE, AFTER)] LINSERT.Where where)
        {
            Key key = Guid.NewGuid().ToString();
            var push = new RPUSH(key, "One", "Two", "Three");
            await fixture.ExecuteAsync(push).ConfigureAwait(false);
            var sut = new LINSERT(key, where, "Four", "NewElement");

            var result = await fixture.ExecuteAsync(sut).ConfigureAwait(false);

            result.Reply.Should().Be(-1);
        }

        [Test]
        public async Task Reply_New_Length_Against_List_With_Pivot([Values(BEFORE, AFTER)] LINSERT.Where where)
        {
            Key key = Guid.NewGuid().ToString();
            var push = new RPUSH(key, "One", "Two", "Three");
            await fixture.ExecuteAsync(push).ConfigureAwait(false);
            var sut = new LINSERT(key, where, "One", "NewElement");

            var result = await fixture.ExecuteAsync(sut).ConfigureAwait(false);

            result.Reply.Should().Be(4);
        }
    }
}