using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Rediska.Commands.Strings;
using Rediska.Protocol;
using Rediska.Tests.Utilities;

namespace Rediska.Tests.Transactions
{
    [Explicit("smoke")]
    public sealed class Smoke
    {
        [Test]
        public async Task Test()
        {
            var connection = new LoggingConnection(
                new SimpleConnection()
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