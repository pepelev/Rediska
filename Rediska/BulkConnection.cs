namespace Rediska
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Protocol;
    using Utils;

    public sealed class BulkConnection
    {
        private readonly Connection connection;
        private TaskCompletionSource<bool> completionSource = new TaskCompletionSource<bool>();
        private readonly List<Pair> commands = new List<Pair>();

        public BulkConnection(Connection connection)
        {
            this.connection = connection;
        }

        public Resource<Connection.Response> Send(DataType command)
        {
            var response = new Response(completionSource);
            commands.Add(new Pair(command, response));
            return response;
        }

        public async Task FlushAsync()
        {
            for (var i = 0; i < commands.Count; i++)
            {
                var command = commands[i];
                // todo dispose
                var response = await connection.SendAsync(command.Request).ConfigureAwait(false);
                command.Response.SetResponse(response);
            }

            completionSource.SetResult(true);
            commands.Clear();
            completionSource = new TaskCompletionSource<bool>();
        }

        private struct Pair
        {
            public Pair(DataType request, Response response)
            {
                Request = request;
                Response = response;
            }

            public DataType Request { get; }
            public Response Response { get; }
        }

        private sealed class Response : Connection.Response
        {
            private readonly TaskCompletionSource<bool> completionSource;
            private Connection.Response response;

            public Response(TaskCompletionSource<bool> completionSource)
            {
                this.completionSource = completionSource;
            }

            public override async Task<DataType> ReadAsync()
            {
                await completionSource.Task.ConfigureAwait(false);
                return await response.ReadAsync().ConfigureAwait(false);
            }

            public void SetResponse(Connection.Response response)
            {
                this.response = response;
            }
        }
    }
}