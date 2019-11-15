using System.IO;
using System.Threading.Tasks;
using Rediska.Protocol;
using Rediska.Protocol.Outputs;
using Rediska.Utils;

namespace Rediska
{
    using System.Configuration;
    using Commands;
    using Commands.Utility;

    public sealed class NoReplyConnection : Connection
    {
        private readonly Response response;
        private readonly Stream stream;

        public NoReplyConnection(Response response, Stream stream)
        {
            this.response = response;
            this.stream = stream;
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
            return response;
        }
    }

    public static class NoReplyConnectionResponse
    {
        public static async Task FireAndForgetAsync<T>(this NoReplyConnection connection, Command<T> command)
        {
            await connection.ExecuteAsync(
                new IgnoringResponse<T>(command)
            ).ConfigureAwait(false);
        }
    }
}