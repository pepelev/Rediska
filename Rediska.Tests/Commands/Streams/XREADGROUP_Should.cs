namespace Rediska.Tests.Commands.Streams
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fixtures;
    using FluentAssertions;
    using NUnit.Framework;
    using Protocol;
    using Protocol.Visitors;
    using Rediska.Commands;
    using Rediska.Commands.Auxiliary;
    using Rediska.Commands.Streams;
    using static Rediska.Commands.Streams.XGROUP.CREATE.Mode;
    using static Rediska.Commands.Streams.XREADGROUP.Mode;
    using Id = Rediska.Commands.Streams.Id;

    [TestFixtureSource(typeof(ConnectionCollection))]
    public sealed class XREADGROUP_Should
    {
        private readonly Connection connection;
        private Fixture fixture;

        public XREADGROUP_Should(Connection connection)
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
        public async Task Return_Single_Entry()
        {
            var key = fixture.NewKey();
            var group = new GroupName("group-1");
            var xadd = new XADD(key, ("name", "value"));
            var xgroupCreate = new XGROUP.CREATE(key, group, Offset.FromId(Id.Minimum), NotCreateStream);
            await fixture.ExecuteAsync(xadd).ConfigureAwait(false);
            await fixture.ExecuteAsync(xgroupCreate).ConfigureAwait(false);

            var sut = new XREADGROUP.New(group, "consumer-1", Count.Unbound, NotRequireAcknowledgment, key);
            var entries = await fixture.ExecuteAsync(sut).ConfigureAwait(false);
            Equals(entries, new Stream(key, new Entry(("name", "value"))));
        }

        [Test]
        public async Task Useless_Dollar()
        {
            var key = fixture.NewKey();
            const string group = "group";
            var xgroupCreate = new XGROUP.CREATE(key, group, Offset.FromId(Id.Minimum), CreateStreamIfNotExists);
            await fixture.ExecuteAsync(xgroupCreate).ConfigureAwait(false);

            var sut = new PlainCommand("XREADGROUP", "GROUP", group, "consumer", "STREAMS", key.ToBytes(), "$");
            var response = await fixture.ExecuteAsync(sut).ConfigureAwait(false);
            response.Should().Be(
                new Error(
                    "ERR The $ ID is meaningless in the context of XREADGROUP: "
                    + "you want to read the history of this consumer by specifying "
                    + "a proper ID, or use the > ID to get new messages. The $ ID "
                    + "would just return an empty result set."
                )
            );
        }

        [Test]
        public async Task Return_Error_When_Run_Against_Not_Existing_Stream()
        {
            var sut = new XREADGROUP("group", "consumer", Count.Unbound, ("key", Id.Minimum))
                .WithRawResponse();
            var response = await fixture.ExecuteAsync(sut).ConfigureAwait(false);
            response.Should().Be(
                new Error("NOGROUP No such key 'key' or consumer group 'group' in XREADGROUP with GROUP option")
            );
        }

        [Test]
        public async Task Return_Error_When_Any_Of_Streams_Does_Not_Exists()
        {
            var key = fixture.NewKey();
            const string group = "group";
            var xgroupCreate = new XGROUP.CREATE(key, group, Offset.FromId(Id.Minimum), CreateStreamIfNotExists);
            await fixture.ExecuteAsync(xgroupCreate).ConfigureAwait(false);

            var sut = new XREADGROUP(
                group,
                "consumer",
                Count.Unbound,
                (key, Id.Minimum),
                ("non-existing-key", Id.Minimum)
            ).WithRawResponse();

            var response = await fixture.ExecuteAsync(sut).ConfigureAwait(false);
            response.Should().Be(
                new Error("NOGROUP No such key 'non-existing-key' or consumer group 'group' in XREADGROUP with GROUP option")
            );
        }

        [Test]
        public async Task Exploratory_1()
        {
            var key = fixture.NewKey();
            var group = new GroupName("group-1");
            var xadd = new XADD(key, ("name", "value"));
            var xgroupCreate = new XGROUP.CREATE(key, group, Offset.FromId(Id.Minimum), NotCreateStream);
            await fixture.ExecuteAsync(xadd).ConfigureAwait(false);
            await fixture.ExecuteAsync(xgroupCreate).ConfigureAwait(false);

            var @new = new PlainCommand("XREADGROUP", "GROUP", "group-1", "consumer", "STREAMS", key.ToBytes(), ">")
                .WithResponseStructure(CompositeVisitors.StreamEntriesList);
            var entries = await fixture.ExecuteAsync(@new).ConfigureAwait(false);
            Equals(entries, new Stream(key, new Entry(("name", "value"))));
        }

        private static void Equals(IReadOnlyList<Entries> entries, params Stream[] streams)
        {
            entries.Should().HaveCount(streams.Length);
            for (var i = 0; i < entries.Count; i++)
            {
                var expected = streams[i];
                var actual = entries[i];
                actual.Stream.ToBytes().Should().Equal(expected.Key.ToBytes());
                actual.Should().HaveCount(expected.Entries.Length);
                for (var j = 0; j < actual.Count; j++)
                {
                    var expectedEntry = expected.Entries[j];
                    var actualEntry = actual[j];
                    actualEntry.Should().Equal(expectedEntry);
                }
            }
        }
    }

    [TestFixtureSource(typeof(ConnectionCollection))]
    public sealed class XREADGROUP_BLOCK_Should
    {
        private readonly Connection connection;
        private Fixture fixture;

        public XREADGROUP_BLOCK_Should(Connection connection)
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
        public async Task Not_Block_If_One_Of_Ids_Is_Not_Greater_Than()
        {
            var key1 = fixture.NewKey("key1");
            var key2 = fixture.NewKey("key2");
            const string group = "group";

            foreach (var key in new[] {key1, key2})
            {
                var xgroupCreate = new XGROUP.CREATE(key, group, Offset.FromId(Id.Minimum), CreateStreamIfNotExists);
                await fixture.ExecuteAsync(xgroupCreate).ConfigureAwait(false);
            }

            var sut = new PlainCommand(
                "XREADGROUP",
                "GROUP",
                "group",
                "consumer",
                "BLOCK",
                "0",
                "STREAMS",
                key1.ToBytes(),
                key2.ToBytes(),
                ">",
                "100"
            );
            var response = await fixture.ExecuteAsync(sut).ConfigureAwait(false);
            response.Should().Be(
                new PlainArray(
                    new PlainArray(
                        key2.ToBulkString(BulkStringFactory.Plain),
                        Array.Empty
                    ) as DataType
                )
            );
        }
    }
}