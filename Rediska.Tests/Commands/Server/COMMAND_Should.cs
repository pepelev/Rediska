namespace Rediska.Tests.Commands.Server
{
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Rediska.Commands.Server;
    using Sets;

    [TestFixtureSource(typeof(ConnectionCollection))]
    public class COMMAND_Should
    {
        private readonly Connection connection;

        public COMMAND_Should(Connection connection)
        {
            this.connection = connection;
        }

        [Test]
        public async Task METHOD()
        {
            var commands = await connection.ExecuteAsync(COMMAND.Singleton).ConfigureAwait(false);
        }
    }
}