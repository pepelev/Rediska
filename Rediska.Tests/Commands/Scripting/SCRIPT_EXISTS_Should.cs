namespace Rediska.Tests.Commands.Scripting
{
    using System.Threading.Tasks;
    using Fixtures;
    using FluentAssertions;
    using NUnit.Framework;
    using Rediska.Commands.Scripting;

    [TestFixtureSource(typeof(ConnectionCollection))]
    public class SCRIPT_EXISTS_Should
    {
        private readonly Connection connection;

        public SCRIPT_EXISTS_Should(Connection connection)
        {
            this.connection = connection;
        }

        [Test]
        public async Task Give_Script_Loaded()
        {
            var load = new SCRIPT.LOAD("return 42");
            await connection.ExecuteAsync(load).ConfigureAwait(false);
            var return42Hash = Sha1.Parse("1fa00e76656cc152ad327c13fe365858fd7be306");
            var nonExistingHash = Sha1.Parse("1fa00e76656cc152ad327c13fe365858fd7be307"); // last char is different

            var response = await connection.ExecuteAsync(
                new SCRIPT.EXISTS(return42Hash, nonExistingHash)
            ).ConfigureAwait(false);

            response.Should().Equal(
                (return42Hash, true),
                (nonExistingHash, false)
            );
        }
    }
}