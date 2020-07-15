namespace Rediska
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Protocol;
    using Protocol.Outputs;
    using Utils;

    public sealed class PipeliningConnection : Connection
    {
        private readonly Stream stream;
        private readonly AsyncLock writeLock = new AsyncLock();
        private readonly AsyncLock readLock = new AsyncLock();
        private readonly TemporaryResponse response = new TemporaryResponse();

        private readonly ConcurrentDictionary<long, DataType> responses =
            new ConcurrentDictionary<long, DataType>();

        private long requestIndex;
        private long responseIndex;


        public PipeliningConnection(Stream stream)
        {
            this.stream = stream;
        }

        public override async Task<Response> SendAsync(DataType command, CancellationToken token)
        {
            var temporaryStream = new MemoryStream();
            command.Write(new StreamOutput(temporaryStream));
            temporaryStream.Position = 0;
            long currentIndex;
            using (await writeLock.AcquireAsync(CancellationToken.None).ConfigureAwait(false))
            {
                await temporaryStream.CopyToAsync(stream).ConfigureAwait(false);
                currentIndex = requestIndex;
                requestIndex++;
            }

            // todo могут на возвращаемом Resource позвать Dispose, зачем я тогда разбираю ответ?
            return new ThisResponse(ReadResponseAsync(currentIndex));
        }

        private async Task<DataType> ReadResponseAsync(long currentRequestIndex)
        {
            if (responses.TryRemove(currentRequestIndex, out var result))
            {
                return result;
            }

            var buffer = new byte[1024 * 80];
            using (await readLock.AcquireAsync(CancellationToken.None).ConfigureAwait(false))
            {
                while (responseIndex <= currentRequestIndex)
                {
                    var count = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                    var inputs = response.Feed(new ArraySegment<byte>(buffer, 0, count));
                    if (inputs.Count == 0)
                    {
                        continue;
                    }

                    DataType returns = null;
                    foreach (var input in inputs)
                    {
                        // todo можно не читать сразу
                        var inputResponse = new InputResponse(input);
                        var readResponse = await inputResponse.ReadAsync().ConfigureAwait(false);
                        if (responseIndex == currentRequestIndex)
                        {
                            returns = readResponse;
                        }
                        else
                        {
                            responses[responseIndex] = readResponse;
                        }

                        responseIndex++;
                    }

                    if (!(returns is null))
                    {
                        return returns;
                    }
                }
            }

            if (responses.TryRemove(currentRequestIndex, out result))
            {
                return result;
            }

            throw new InvalidOperationException("This exception is likely indicates a bug");
        }

        private sealed class ThisResponse : Response
        {
            private readonly Task<DataType> task;

            public ThisResponse(Task<DataType> task)
            {
                this.task = task;
            }

            public override Task<DataType> ReadAsync() => task;
        }
    }
}