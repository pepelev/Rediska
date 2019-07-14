using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Rediska.Tests
{
    public abstract class Queue
    {
        public abstract Task<Input> EnqueueAsync(byte[] content);
    }

    public sealed class Response
    {
        private readonly ChunkList<byte> chunks;
        private ChunkList<byte>.Cursor start;
        private ChunkList<byte>.Cursor end;

        public void Feed(ArraySegment<byte> chunk)
        {
            chunks.Add(chunk);
        }

        public void MoveNext()
        {
            if (!IsReady)
                throw new InvalidOperationException($"Unable to perform {nameof(MoveNext)} when current segment is not ready");

            end.Cut();
            start = end;
            IsReady = false;
        }

        public bool IsReady { get; private set; }

        public Input Current
        {
            get
            {
                if (!IsReady)
                    throw new InvalidOperationException($"Unable to perform {nameof(Current)} when current segment is not ready");

                return null; // todo
            }
        }
    }

    public sealed class SemaphoreQueue : Queue
    {
        private readonly Stream stream;
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private readonly Response response;

        public SemaphoreQueue(Stream stream)
        {
            this.stream = stream;
        }

        public override async Task<Input> EnqueueAsync(byte[] content)
        {
            await semaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                await stream.WriteAsync(content, 0, content.Length).ConfigureAwait(false);

                do
                {
                    var result = new byte[1024];
                    var read = await stream.ReadAsync(result, 0, result.Length).ConfigureAwait(false);
                    var chunk = new ArraySegment<byte>(result, 0, read);
                    response.Feed(chunk);
                } while (!response.IsReady);

                return response.Current;
            }
            finally
            {
                semaphore.Release();
            }
        }
    }

    public sealed class MyQueue : Queue
    {
        private readonly ConcurrentQueue<byte[]> queue;
        private readonly Stream stream;
        private int state;

        public override async Task<Input> EnqueueAsync(byte[] content)
        {
            var state = this.state;
            if (state == Busy)
            {
                // enqueue
            }
            else
            {
                if (Interlocked.CompareExchange(ref state, Busy, Free) == Free)
                {
                    // go ahead
                }
            }

            return null; // todo
        }

        private const int Free = 0;
        private const int Busy = 1;
    }
}