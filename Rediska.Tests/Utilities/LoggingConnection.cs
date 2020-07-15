using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rediska.Protocol;
using Rediska.Utils;

namespace Rediska.Tests.Utilities
{
    using System;
    using System.Threading;

    public sealed class LoggingConnection : Connection
    {
        private readonly Connection connection;
        private readonly ConcurrentQueue<Entry> log = new ConcurrentQueue<Entry>();

        public LoggingConnection(Connection connection)
        {
            this.connection = connection;
        }

        public IReadOnlyCollection<Entry> Log => log;

        public override async Task<Response> SendAsync(DataType command, CancellationToken token)
        {
            var requestTime = DateTime.Now;
            var response = await connection.SendAsync(command, token).ConfigureAwait(false);
            var dataType = await response.ReadAsync().ConfigureAwait(false);
            var responseTime = DateTime.Now;
            log.Enqueue(
                new Entry(
                    requestTime,
                    command,
                    dataType,
                    responseTime
                )
            );
            return new ConstResponse(dataType);
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