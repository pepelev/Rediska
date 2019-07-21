using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Rediska.Tests
{
    public sealed class Redis
    {
        private readonly TcpClient tcp;
        private readonly Queue queue;
        private readonly NetworkStream stream;

        public Redis()
        {
            tcp = new TcpClient
            {
                NoDelay = true
            };
            tcp.Connect(IPAddress.Loopback, 6379);
            stream = tcp.GetStream();
        }

        public async Task<T> ExecuteAsync<T>(Command<T> command)
        {
            var bulkWriteStream = new BulkWriteStream(
                stream,
                new MemoryStream()
            );
            var output = new CompoundOutput(
                new VerifyingOutput(),
                new StreamOutput(
                    bulkWriteStream
                )
            );
            command.Request.Write(output);
            await bulkWriteStream.FlushAsync().ConfigureAwait(false);
            using (var response = await ReadResponseAsync().ConfigureAwait(false))
            {
                //return command.Read(response);
                throw new NotImplementedException();
            }
        }

        private async Task<Resource<Input>> ReadResponseAsync()
        {
            var response = new TemporaryResponse();
            var buffer = new byte[1024 * 80];
            while (true)
            {
                var count = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                var inputs = response.Feed(new ArraySegment<byte>(buffer, 0, count));
                if (inputs.Count > 0)
                {
                    return new Resource<Input>(
                        inputs.Single(),
                        new MemoryStream()
                    );
                }
            }
        }
    }
}