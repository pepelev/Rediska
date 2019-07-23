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
    public sealed class SADDShould
    {
        private LoggingConnection connection;
        private Key key;

        [SetUp]
        public async Task SetUpAsync()
        {
            var tcp = new TcpClient
            {
                NoDelay = true
            };
            var ip = new IPAddress(
                new byte[] {192, 168, 56, 1}
            );
            await tcp.ConnectAsync(ip, 27_000).ConfigureAwait(false);
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
    }
}