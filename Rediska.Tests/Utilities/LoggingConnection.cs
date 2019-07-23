using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rediska.Protocol.Requests;
using Rediska.Utils;

namespace Rediska.Tests.Utilities
{
    public sealed class LoggingConnection : Connection
    {
        private readonly Connection connection;
        private readonly ConcurrentQueue<Entry> log = new ConcurrentQueue<Entry>();

        public LoggingConnection(Connection connection)
        {
            this.connection = connection;
        }

        public IReadOnlyCollection<Entry> Log => log;

        public override async Task<Resource<Response>> SendAsync(DataType command)
        {
            var response = await connection.SendAsync(command).ConfigureAwait(false);
            var dataType = await response.Value.ReadAsync().ConfigureAwait(false);
            log.Enqueue(
                new Entry(
                    command,
                    dataType
                )
            );
            return new Resource<Response>(
                new ConstResponse(dataType),
                response
            );
        }

        public struct Entry
        {
            public Entry(DataType request, Protocol.Responses.DataType response)
            {
                Request = request;
                Response = response;
            }

            public DataType Request { get; }
            public Protocol.Responses.DataType Response { get; }
        }
    }
}