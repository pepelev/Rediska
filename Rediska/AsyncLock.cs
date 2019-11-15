namespace Rediska
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class AsyncLock
    {
        private volatile SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        public bool IsTaken
        {
            get
            {
                var semaphoreSlim = semaphore;
                if (semaphoreSlim == null)
                {
                    throw new ObjectDisposedException(nameof(AsyncLock));
                }

                return semaphoreSlim.CurrentCount == 0;
            }
        }

        public async Task<Session> AcquireAsync(CancellationToken token)
        {
            var semaphoreSlim = semaphore;
            if (semaphoreSlim == null)
            {
                throw new ObjectDisposedException(nameof(AsyncLock));
            }

            var session = new Session(this, token);
            await semaphoreSlim.WaitAsync(token).ConfigureAwait(false);
            return session;
        }

        public void Dispose()
        {
            var oldValue = Interlocked.Exchange(ref semaphore, null);
            oldValue?.Dispose();
        }

        public sealed class Session : IDisposable
        {
            private AsyncLock @lock;

            public Session(
                AsyncLock @lock,
                CancellationToken token)
            {
                this.@lock = @lock;
                CancellationToken = token;
            }

            public void Dispose()
            {
                var oldValue = Interlocked.Exchange(ref @lock, null);
                oldValue?.semaphore?.Release();
            }

            public CancellationToken CancellationToken { get; }
        }
    }
}