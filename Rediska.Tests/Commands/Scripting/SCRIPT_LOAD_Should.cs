namespace Rediska.Tests.Commands.Scripting
{
    using System.Threading.Tasks;
    using Fixtures;
    using FluentAssertions;
    using NUnit.Framework;
    using Rediska.Commands.Scripting;

    [TestFixtureSource(typeof(ConnectionCollection))]
    public class SCRIPT_LOAD_Should
    {
        private readonly Connection connection;

        public SCRIPT_LOAD_Should(Connection connection)
        {
            this.connection = connection;
        }

        [Test]
        public async Task Test()
        {
            var scriptHash = await connection.ExecuteAsync(
                new SCRIPT.LOAD("return 42")
            ).ConfigureAwait(false);

            scriptHash.Should().Be(Sha1.Parse("1fa00e76656cc152ad327c13fe365858fd7be306"));
        }

        [Test]
        [TestCase("return 42")]
        [TestCase("return 'Кошка'")]
        public async Task Match_With_Local_Hash(string script)
        {
            var scriptHash = await connection.ExecuteAsync(
                new SCRIPT.LOAD(script)
            ).ConfigureAwait(false);

            scriptHash.Should().Be(Sha1.Create(script));
        }
    }
}