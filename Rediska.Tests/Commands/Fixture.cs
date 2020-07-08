namespace Rediska.Tests.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using NUnit.Framework.Interfaces;
    using Rediska.Commands;
    using Rediska.Commands.Keys;
    using Utilities;

    public sealed class Fixture
    {
        private static TestContext TestContext => TestContext.CurrentContext;
        private readonly LoggingConnection connection;
        private readonly List<Key> keys = new List<Key>();

        public Fixture(Connection connection)
        {
            this.connection = new LoggingConnection(connection);
        }

        public Key NewKey()
        {
            var content = $"{TestContext.Test.FullName}:{Guid.NewGuid()}";
            var key = new Key.Utf8(content);
            keys.Add(key);
            return key;
        }

        public async Task<T> ExecuteAsync<T>(Command<T> command)
        {
            return await connection.ExecuteAsync(command).ConfigureAwait(false);
        }

        public async Task TearDownAsync()
        {
            if (TestContext.Result.Outcome.Status == TestStatus.Failed)
            {
                PrintHistory();
            }
            else
            {
                await CleanupAsync().ConfigureAwait(false);
            }
        }

        private void PrintHistory()
        {
            var entries = connection.Log.Select(
                (entry, index) =>
                {
                    var request = $"{index + 1}) {entry.RequestedAt:hh:mm:ss.zzz}: -> {entry.Request}";
                    var response = $"{index + 1}) {entry.ResponseReceivedAt:hh:mm:ss.zzz}: <- {entry.Response}";
                    return $"{request}{Environment.NewLine}{response}";
                }
            );

            var log = string.Join(
                Environment.NewLine,
                entries
            );
            TestContext.Out.WriteLine(log);
        }

        private async Task CleanupAsync()
        {
            if (keys.Count == 0)
                return;

            var command = new UNLINK(keys);
            await connection.ExecuteAsync(command).ConfigureAwait(false);
        }
    }
}