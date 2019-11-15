namespace Rediska.Tests.Commands.Lists
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using NUnit.Framework;
    using Rediska.Commands.Lists;
    using static Rediska.Commands.Lists.LINSERT.Where;

    public sealed class LINSERT_Should
    {
        private SimpleConnection connection;

        [SetUp]
        public void SetUp()
        {
            connection = new SimpleConnection();
        }

        [Test]
        public async Task Reply_With_Zero_Against_Empty_List([Values(BEFORE, AFTER)] LINSERT.Where where)
        {
            var sut = new LINSERT("non-existent-key", where, "Pivot", "NewElement");

            var result = await connection.ExecuteAsync(sut).ConfigureAwait(false);

            result.Reply.Should().Be(0);
        }

        [Test]
        public async Task Reply_MinusOne_Against_List_Without_Pivot([Values(BEFORE, AFTER)] LINSERT.Where where)
        {
            Key key = Guid.NewGuid().ToString();
            var push = new RPUSH(key, "One", "Two", "Three");
            await connection.ExecuteAsync(push).ConfigureAwait(false);
            var sut = new LINSERT(key, where, "Four", "NewElement");

            var result = await connection.ExecuteAsync(sut).ConfigureAwait(false);

            result.Reply.Should().Be(-1);
        }

        [Test]
        public async Task Reply_New_Length_Against_List_With_Pivot([Values(BEFORE, AFTER)] LINSERT.Where where)
        {
            Key key = Guid.NewGuid().ToString();
            var push = new RPUSH(key, "One", "Two", "Three");
            await connection.ExecuteAsync(push).ConfigureAwait(false);
            var sut = new LINSERT(key, where, "One", "NewElement");

            var result = await connection.ExecuteAsync(sut).ConfigureAwait(false);

            result.Reply.Should().Be(4);
        }
    }
}