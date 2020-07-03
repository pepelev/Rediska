namespace Rediska.Tests.Commands.Server
{
    using System;
    using System.Linq;
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
        public async Task Print_All_Commands()
        {
            var commands = await connection.ExecuteAsync(COMMAND.Singleton).ConfigureAwait(false);
            var descriptions = commands.OrderBy(command => command.Name, StringComparer.InvariantCultureIgnoreCase);
            var nl = Environment.NewLine;
            var result = string.Join(
                nl + nl,
                descriptions.Select(
                    description => $"{description.Name}{nl}" +
                                   $"{description.Aritry}{nl}" +
                                   $"{description.Flags}{nl}" +
                                   $"{description.FirstKeyPosition}{nl}" +
                                   $"{description.LastKeyPosition}{nl}" +
                                   $"{description.KeyStepCount}{nl}" +
                                   $"{description.Categories}"
                )
            );

            // todo approval
            Console.WriteLine(result);
        }
    }
}