namespace Rediska.Tests.Examples
{
    using System.Net;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Rediska.Commands.Strings;

    public class SimpleConnectionCreation
    {
        [Test]
        public async Task Subject()
        {
            var factory = new SimpleConnectionFactory();
            var endPoint = new IPEndPoint(IPAddress.Loopback, 6379);
            using (var connectionResource = await factory.CreateAsync(endPoint))
            {
                var connection = connectionResource.Value;

                var set = new SET("users:12:score", "50");
                await connection.ExecuteAsync(set);
                var incr = new INCR("users:12:score");
                await connection.ExecuteAsync(incr);
                var get = new GET("users:12:score");
                var userScore = await connection.ExecuteAsync(get); // 51
            }
        }
    }
}