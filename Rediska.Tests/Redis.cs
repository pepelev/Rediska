using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
            var bytes = new byte[1024];
            var request = new PlainOutput(bytes);
            command.Write(request);
            await stream.WriteAsync(bytes, 0, request.Position).ConfigureAwait(false);
            using (var response = await ReadResponseAsync(stream).ConfigureAwait(false))
            {
                return command.Read(response);
            }
        }

        private async Task<Resource<Input>> ReadResponseAsync(NetworkStream stream)
        {
            var buffer = new byte[1024];
            var count = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
            var binaryReader = new BinaryReader(
                new MemoryStream(buffer, 0, count),
                Encoding.ASCII
            );
            return new Resource<Input>(
                new PlainInput(
                    binaryReader
                ),
                binaryReader
            );
        }
    }
}