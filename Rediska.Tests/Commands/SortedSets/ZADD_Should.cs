namespace Rediska.Tests.Commands.SortedSets
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fixtures;
    using FluentAssertions;
    using NUnit.Framework;
    using Protocol;
    using Rediska.Commands;
    using Rediska.Commands.Auxiliary;
    using Rediska.Commands.Keys;
    using Rediska.Commands.SortedSets;
    using Utilities;
    using static Rediska.Commands.SortedSets.ZADD.Mode;
    using Range = Rediska.Commands.Range;

    [TestFixtureSource(typeof(ConnectionCollection))]
    public class ZADD_Should
    {
        public static TestCaseData[] RequestCases =
        {
            Case(new ZADD("key", (10, "member")), "ZADD key 10 member"),
            Case(new ZADD("key", (double.PositiveInfinity, "member")), "ZADD key +inf member"),
            Case(new ZADD("key", (double.NegativeInfinity, "member")), "ZADD key -inf member"),
            Case(new ZADD("key-2", AddOrUpdateScore, (10, "member")), "ZADD key-2 10 member"),
            Case(new ZADD("key-5", UpdateScoreOnly, (10.105, "member")), "ZADD key-5 XX 10.105 member"),
            Case(new ZADD("key-7", AddMembersOnly, (0.3, "new-member")), "ZADD key-7 NX 0.3 new-member"),
            Case(new ZADD("key", AddMembersOnly, (10, "new-member"), (15, "another")), "ZADD key NX 10 new-member 15 another"),
        };

        private static TestCaseData Case(ZADD command, string expectation)
        {
            var expectedBulkString = PlainCommand.Parse(expectation).Request(BulkStringFactory.Plain);
            return new TestCaseData(command).Returns(expectedBulkString);
        }

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
        [TestCaseSource(nameof(RequestCases))]
        public IEnumerable<BulkString> Build_Request(ZADD command) => command.Request(BulkStringFactory.Plain);

        [Test]
        public void Not_Allow_To_Build_Useless_Command()
        {
            Assert.Throws<ArgumentException>(
                () => { _ = new ZADD("key"); }
            );
        }

        [Test]
        public void Not_Allow_To_Build_Useless_Command_With_Mode([Values] ZADD.Mode mode)
        {
            Assert.Throws<ArgumentException>(
                () => { _ = new ZADD("key", mode); }
            );
        }

        [Test]
        public void Not_Allow_To_Build_Command_With_Nan_Score([Values] ZADD.Mode mode)
        {
            Assert.Throws<ArgumentException>(
                () => { _ = new ZADD("key", mode, (double.NaN, "member")); }
            );
        }

        [Test]
        public async Task Create_List_By_Adding_First_Member()
        {
            var key = fixture.NewKey();
            await fixture.ExecuteAsync(
                new ZADD(key, (42, "42"))
            ).ConfigureAwait(false);

            var items = await fixture.ExecuteAsync(new ZRANGE.WITHSCORES(key, Range.Whole)).ConfigureAwait(false);
            items.Should().Equal(("42", 42));
        }

        [Test]
        public async Task Add_Several_Members()
        {
            var key = fixture.NewKey();
            await fixture.ExecuteAsync(
                new ZADD(key, (42, "Red"), (53.3, "Green"), (4.7, "Blue"))
            ).ConfigureAwait(false);

            var items = await fixture.ExecuteAsync(new ZRANGE.WITHSCORES(key, Range.Whole)).ConfigureAwait(false);
            items.Should().Be(("Red", 42), ("Green", 53.3), ("Blue", 4.7));
        }

        [Test]
        public async Task Overwrite_Score_For_Mode(
            [Values(UpdateScoreOnly, AddOrUpdateScore)]
            ZADD.Mode mode)
        {
            var key = fixture.NewKey();
            await fixture.ExecuteAsync(
                new ZADD(key, (42, "Apple"), (53.3, "Banana"), (4.7, "Orange"))
            ).ConfigureAwait(false);

            await fixture.ExecuteAsync(
                new ZADD(key, mode, (37, "Apple"), (23, "Orange"))
            ).ConfigureAwait(false);

            var items = await fixture.ExecuteAsync(new ZRANGE.WITHSCORES(key, Range.Whole)).ConfigureAwait(false);
            items.Should().Be(("Apple", 37), ("Banana", 53.3), ("Orange", 23));
        }

        [Test]
        public async Task Not_Overwrite_Score_For_AddMembersOnly_Mode()
        {
            var key = fixture.NewKey();
            await fixture.ExecuteAsync(
                new ZADD(key, (42, "Apple"), (53.3, "Banana"), (4.7, "Orange"))
            ).ConfigureAwait(false);

            await fixture.ExecuteAsync(
                new ZADD(key, AddMembersOnly, (37, "Apple"), (23, "Orange"))
            ).ConfigureAwait(false);

            var items = await fixture.ExecuteAsync(new ZRANGE.WITHSCORES(key, Range.Whole)).ConfigureAwait(false);
            items.Should().Be(("Apple", 42), ("Banana", 53.3), ("Orange", 4.7));
        }

        [Test]
        public async Task Do_Nothing_With_UpdateScoreOnly_Mode_When_Key_Does_Not_Exists()
        {
            var key = fixture.NewKey();
            await fixture.ExecuteAsync(
                new ZADD(key, UpdateScoreOnly, (10, "Member"))
            ).ConfigureAwait(false);

            var exists = await fixture.ExecuteAsync(new EXISTS(key)).ConfigureAwait(false);
            exists.Should().Be(0);
        }
    }
}