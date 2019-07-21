using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Rediska.Tests
{
    public sealed class PipeliningRedis
    {
        private readonly Cashier cashier = new Cashier();
        private readonly SemaphoreSlim reader = new SemaphoreSlim(1, 1);
        private readonly NetworkStream stream;

        public PipeliningRedis(NetworkStream stream)
        {
            this.stream = stream;
        }

        private readonly ConcurrentDictionary<long, TaskCompletionSource<Input>> received = new ConcurrentDictionary<long, TaskCompletionSource<Input>>();
        private readonly TemporaryResponse response = new TemporaryResponse();
        private readonly byte[] buffer = new byte[1024 * 80];
        private readonly AtomicLong activeResponseReaderNumber = new AtomicLong(0);
        private readonly AtomicLong responsesRead = new AtomicLong(0);

        public async Task Do()
        {
            var tcs = new TaskCompletionSource<long>();
            tcs.SetResult(-1);

            {
                await tcs.Task.ConfigureAwait(false);


            }
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
            long number;
            using (var ticket = await cashier.AcquireTicketAsync().ConfigureAwait(false))
            {
                await bulkWriteStream.FlushAsync().ConfigureAwait(false);
                number = ticket.Number;
            }

            while (true)
            {
                switch (TryAcquire(number))
                {
                    case AcquisitionResult.Acquired:

                        var counter = responsesRead.Read();
                        do
                        {
                            var responses = await ReadResponsesAsync().ConfigureAwait(false);
                            foreach (var item in responses)
                            {
                                var tcs = received.GetOrAdd(++counter, _ => new TaskCompletionSource<Input>());
                                tcs.SetResult(item);
                            }
                        } while (counter < number);
                        activeResponseReaderNumber.Write(-1);

                        received.TryRemove(number, out var tcs2);
                        var result = await tcs2.Task.ConfigureAwait(false);
                        //return command.Read(result);
                        throw new NotImplementedException();

                    case AcquisitionResult.BusyByNext:
                        var source = received.GetOrAdd(number, _ => new TaskCompletionSource<Input>());
                        var input = await source.Task.ConfigureAwait(false);
                        received.TryRemove(number, out _);
                        //return command.Read(input);
                        throw new NotImplementedException();

                    case AcquisitionResult.BusyByPrevious:
                        // todo wait
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private AcquisitionResult TryAcquire(long number)
        {
            var result = activeResponseReaderNumber.IfEqualTo(-1).Write(number);
            if (result.Success)
                return AcquisitionResult.Acquired;

            return result.OldValue > number
                ? AcquisitionResult.BusyByNext
                : AcquisitionResult.BusyByPrevious;
        }

        private enum AcquisitionResult
        {
            Acquired,
            BusyByNext,
            BusyByPrevious
        }

        public int PendingQueriesCount => throw new NotImplementedException();

        private async Task<IReadOnlyCollection<Input>> ReadResponsesAsync()
        {
            while (true)
            {
                var count = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                var inputs = response.Feed(new ArraySegment<byte>(buffer, 0, count));
                if (inputs.Count > 0)
                {
                    return inputs;
                }
            }
        }
    }
}