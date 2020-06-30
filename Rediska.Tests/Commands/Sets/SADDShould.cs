using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using Rediska.Commands.Keys;
using Rediska.Commands.Sets;
using Rediska.Protocol;
using Rediska.Protocol.Visitors;
using Rediska.Tests.Utilities;
using Array = System.Array;

namespace Rediska.Tests.Commands.Sets
{
    [Category("RealRedis")]
    [TestFixtureSource(typeof(RedisCollection))]
    public sealed class SADDShould
    {
        private readonly IPEndPoint endpoint;
        private LoggingConnection connection;
        private Key key;
        private TcpClient tcp;

        public SADDShould(IPEndPoint endpoint)
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

        [Test]
        public async Task AddNoElements()
        {
            var sut = new SADD(key, Array.Empty<BulkString>());

            var exception = Assert.ThrowsAsync<VisitException>(
                () => connection.ExecuteAsync(sut)
            );

            exception.Subject.Should().Be(
                new Error("ERR wrong number of arguments for 'sadd' command")
            );
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
                    Console.WriteLine($"{entry.RequestedAt:hh:mm:ss.zzz} {index}: -> {entry.Request}");
                    Console.WriteLine($"{entry.ResponseReceivedAt:hh:mm:ss.zzz} {index}: <- {entry.Response}");

                    index++;
                }
            }
        }
    }
}