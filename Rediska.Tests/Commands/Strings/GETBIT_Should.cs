namespace Rediska.Tests.Commands.Strings
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using NUnit.Framework;
    using Protocol;
    using Rediska.Commands.Strings;
    using Rediska.Commands.Utility;
    using Sets;

    [TestFixtureSource(typeof(ConnectionCollection))]
    public sealed class GETBIT_Should
    {
        private readonly Connection connection;

        public GETBIT_Should(Connection connection)
        {
            this.connection = connection;
        }

        [Test]
        [TestCase(-1L)]
        [TestCase((long)uint.MaxValue + 1)]
        public async Task Deny_Out_Of_Range_Offsets(long offset)
        {
            var response = await connection.ExecuteAsync(
                new PlainCommand("GETBIT", "key", offset.ToBulkString())
            ).ConfigureAwait(false);

            response.Should().Be(new Error("ERR bit offset is not an integer or out of range"));
        }

        [Test]
        [TestCase(0U)]
        [TestCase(1_007U)]
        [TestCase(uint.MaxValue)]
        public async Task Allow_UInt_Offsets(uint offset)
        {
            var response = await connection.ExecuteAsync(
                new GETBIT("key", offset)
            ).ConfigureAwait(false);

            response.Should().Be(false);
        }

        [Test]
        [TestCase(0U, ExpectedResult = false)]
        [TestCase(1U, ExpectedResult = false)]
        [TestCase(2U, ExpectedResult = true)]
        [TestCase(3U, ExpectedResult = false)]
        [TestCase(4U, ExpectedResult = false)]
        [TestCase(5U, ExpectedResult = true)]
        [TestCase(6U, ExpectedResult = false)]
        [TestCase(7U, ExpectedResult = true)]
        [TestCase(8U, ExpectedResult = true)]
        [TestCase(9U, ExpectedResult = false)]
        [TestCase(10U, ExpectedResult = true)]
        [TestCase(11U, ExpectedResult = true)]
        [TestCase(12U, ExpectedResult = true)]
        [TestCase(13U, ExpectedResult = true)]
        [TestCase(14U, ExpectedResult = true)]
        [TestCase(15U, ExpectedResult = true)]
        [TestCase(16U, ExpectedResult = false)]
        [TestCase(uint.MaxValue, ExpectedResult = false)]
        public async Task<bool> Return_Requested_Bit(uint offset)
        {
            const string key = "GETBIT-key";
            await connection.ExecuteAsync(
                new SET(key, new PlainBulkString(new byte[] {0b0010_0101, 0b1011_1111}))
            ).ConfigureAwait(false);

            return await connection.ExecuteAsync(
                new GETBIT(key, offset)
            ).ConfigureAwait(false);
        }
    }
}