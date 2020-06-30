using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rediska.Protocol;
using Rediska.Utils;

namespace Rediska.Tests.Utilities
{
    using System;

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
            var requestTime = DateTime.Now;
            var response = await connection.SendAsync(command).ConfigureAwait(false);
            var dataType = await response.Value.ReadAsync().ConfigureAwait(false);
            var responseTime = DateTime.Now;
            log.Enqueue(
                new Entry(
                    requestTime,
                    command,
                    dataType,
                    responseTime
                )
            );
            return response.Move<Response>(
                new ConstResponse(dataType)
            );
        }

        public readonly struct Entry
        {
            public Entry(DateTime requestedAt, DataType request, DataType response, DateTime responseReceivedAt)
            {
                RequestedAt = requestedAt;
                Request = request;
                Response = response;
                ResponseReceivedAt = responseReceivedAt;
            }

            public DateTime RequestedAt { get; }
            public DataType Request { get; }
            public DataType Response { get; }
            public DateTime ResponseReceivedAt { get; }
        }
    }
}