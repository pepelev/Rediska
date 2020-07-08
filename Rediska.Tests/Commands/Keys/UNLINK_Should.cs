using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Rediska.Commands.Keys;
using Rediska.Tests.Fixtures;

namespace Rediska.Tests.Commands.Keys
{
    using Protocol;

    [TestFixtureSource(typeof(ConnectionCollection))]
    public sealed class UNLINK_Should
    {
        private readonly Connection connection;
        private Fixture fixture;

        public UNLINK_Should(Connection connection)
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
        public void Not_Allow_Build_With_Zero_Keys()
        {
            Assert.Throws<ArgumentException>(
                () => _ = new UNLINK()
            );
        }
    }
}