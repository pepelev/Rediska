namespace Rediska.Tests.Commands.Server
{
    using System;
    using System.Threading;
    using Fixtures;
    using FluentAssertions;
    using NUnit.Framework;
    using Rediska.Commands.Server;

    [TestFixtureSource(typeof(ConnectionCollection))]
    public sealed class SHUTDOWN_Should
    {
        private readonly Connection connection;

        public SHUTDOWN_Should(Connection connection)
        {
            this.connection = connection;
        }

        [Test]
        public void Throw_OperationCancelledException()
        {
            using var tokenSource = new CancellationTokenSource(5.Seconds());
            var token = tokenSource.Token;
            Assert.CatchAsync<OperationCanceledException>(
                () => connection.ExecuteAsync(new SHUTDOWN(ShutdownMode.NoSave), token)
            );
        }
    }
}