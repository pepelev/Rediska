using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Rediska.Protocol;
using Rediska.Protocol.Inputs;
using Rediska.Protocol.Outputs;
using Rediska.Utils;

namespace Rediska
{
    using System.Threading;

    public sealed class SimpleConnection : Connection
    {
        private readonly Stream stream;

        public SimpleConnection(Stream stream)
        {
            this.stream = stream;
        }

        public override async Task<Response> SendAsync(DataType command, CancellationToken token)
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
            await bulkWriteStream.FlushAsync(token).ConfigureAwait(false);
            var input = await ReadResponseAsync(token).ConfigureAwait(false);
            return new InputResponse(input);
        }

        private async Task<Input> ReadResponseAsync(CancellationToken token)
        {
            var response = new TemporaryResponse();
            var buffer = new byte[1024 * 80];
            while (true)
            {
                var count = await stream.ReadAsync(buffer, 0, buffer.Length, token).ConfigureAwait(false);
                var inputs = response.Feed(new ArraySegment<byte>(buffer, 0, count));
                if (inputs.Count > 0)
                {
                    return inputs.Single();
                }
            }
        }
    }
}