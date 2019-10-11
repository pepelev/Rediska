namespace Rediska.Tests.Commands.Sets
{
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using NUnit.Framework.Interfaces;
    using Rediska.Commands.Keys;
    using Utilities;
    using Utils;

    public sealed class SimpleConnectionFixture : ConnectionFixture
    {
        private readonly IPEndPoint endPoint;

        private readonly StoringKeyFixture keys = new StoringKeyFixture(
            new TestNameKeyFixture(
                GuidKeyFixture.Singleton
            )
        );

        private Resource<LoggingConnection> connection;

        public SimpleConnectionFixture(IPEndPoint endPoint)
        {
            this.endPoint = endPoint;
        }

        public override KeyFixture Keys => keys;

        public override async Task<Connection> SetUpAsync()
        {
            var tcp = new TcpClient
            {
                NoDelay = true
            };
            await tcp.ConnectAsync(endPoint.Address, endPoint.Port).ConfigureAwait(false);
            var stream = tcp.GetStream();
            var result = new LoggingConnection(
                new SimpleConnection(stream)
            );
            connection = new Resource<LoggingConnection>(result, tcp);
            return result;
        }

        public override async Task TearDownAsync()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            if (status == TestStatus.Passed)
            {
                await connection.Value.ExecuteAsync(new DEL(keys.ToList())).ConfigureAwait(false);
            }

            using (var resource = connection)
            {
                
            }
        }
    }
}