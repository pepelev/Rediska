using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Rediska.Protocol;
using Rediska.Protocol.Requests;
using Rediska.Utils;

namespace Rediska
{
    public sealed class SimpleConnection : Connection
    {
        private TcpClient tcp;
        private NetworkStream stream;

        public SimpleConnection()
        {
            tcp = new TcpClient
            {
                NoDelay = true
            };
            tcp.Connect(IPAddress.Loopback, 6379);
            stream = tcp.GetStream();
        }

        public override async Task<Resource<Response>> SendAsync(DataType command)
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
            command.Write(output);
            await bulkWriteStream.FlushAsync().ConfigureAwait(false);
            var input = await ReadResponseAsync().ConfigureAwait(false);
            return new Resource<Response>(
                new InputResponse(input)
            );
        }

        private async Task<Input> ReadResponseAsync()
        {
            var response = new TemporaryResponse();
            var buffer = new byte[1024 * 80];
            while (true)
            {
                var count = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                var inputs = response.Feed(new ArraySegment<byte>(buffer, 0, count));
                if (inputs.Count > 0)
                {
                    return inputs.Single();
                }
            }
        }
    }
}