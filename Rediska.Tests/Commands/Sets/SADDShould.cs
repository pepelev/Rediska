﻿namespace Rediska.Tests.Commands.Sets
{
    using System.Threading.Tasks;
    using Fixtures;
    using FluentAssertions;
    using NUnit.Framework;
    using Protocol;
    using Protocol.Visitors;
    using Rediska.Commands.Sets;
    using Array = System.Array;

    [TestFixtureSource(typeof(ConnectionCollection))]
    public sealed class SADD_Should
    {
        private readonly Connection connection;
        private Fixture fixture;

        public SADD_Should(Connection connection)
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
        public void AddNoElements()
        {
            var key = fixture.NewKey();
            var sut = new SADD(key, Array.Empty<BulkString>());

            var exception = Assert.ThrowsAsync<VisitException>(
                () => fixture.ExecuteAsync(sut)
            );

            exception.Subject.Should().Be(
                new Error("ERR wrong number of arguments for 'sadd' command")
            );
        }
    }
}