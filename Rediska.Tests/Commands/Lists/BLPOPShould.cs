using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using Rediska.Commands.Keys;
using Rediska.Commands.Lists;
using Rediska.Tests.Commands.Sets;
using Rediska.Tests.Utilities;

namespace Rediska.Tests.Commands.Lists
{
    [Category("RealRedis")]
    [TestFixtureSource(typeof(LocalRedis))]
    public sealed class BLPOPShould
    {
        private readonly IPEndPoint endpoint;
        private LoggingConnection connection;
        private Key key;
        private TcpClient tcp;

        public BLPOPShould(IPEndPoint endpoint)
        {
            this.endpoint = endpoint;
        }

        [SetUp]
        public async Task SetUpAsync()
        {
            tcp = new TcpClient
            {
                NoDelay = true
            };
            await tcp.ConnectAsync(endpoint.Address, endpoint.Port).ConfigureAwait(false);
            connection = new LoggingConnection(
                new SimpleConnection(tcp.GetStream())
            );
            key = TestContext.CurrentContext.Test.Name + Guid.NewGuid();
        }

        [TearDown]
        public async Task TearDownAsync()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            if (status == TestStatus.Passed)
            {
                await connection.ExecuteAsync(new DEL(key)).ConfigureAwait(false);
                return;
            }

            if (status == TestStatus.Failed)
            {
                tcp?.Dispose();

                if (key == null || connection == null)
                    return;

                Console.WriteLine($"Key: {key}");

                var index = 1;
                foreach (var entry in connection.Log)
                {
                    Console.WriteLine($"{index}: -> {entry.Request}");
                    Console.WriteLine($"{index}: <- {entry.Response}");

                    index++;
                }
            }
        }

        [Test]
        public async Task Test()
        {
            var array = await connection.ExecuteAsync(new BLPOP(key, 20)).ConfigureAwait(false);
        }
    }
}