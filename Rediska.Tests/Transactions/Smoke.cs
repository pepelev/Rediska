namespace Rediska.Tests.Transactions
{
    using System.Net;
    using System.Threading.Tasks;
    using FluentAssertions;
    using NUnit.Framework;
    using Protocol;
    using Rediska.Commands.Strings;
    using Utilities;

    [Explicit("smoke")]
    public sealed class Smoke
    {
        [Test]
        public async Task Test()
        {
            var factory = new SimpleConnectionFactory();
            var endPoint = new IPEndPoint(IPAddress.Loopback, 6379);
            var connection = new LoggingConnection(
                (await factory.CreateAsync(endPoint).ConfigureAwait(false)).Value
            );

            var transaction = new SimpleTransaction(
                new BulkConnection(
                    connection
                )
            );

            var key1 = transaction.Enqueue(new GET("key1"));
            var key2 = transaction.Enqueue(new GET("key2"));

            await transaction.ExecuteAsync().ConfigureAwait(false);

            key1.Result.Should().Be(
                new PlainBulkString("100")
            );
        }
    }
}