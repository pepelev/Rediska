namespace Rediska.Tests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Utilities;

    public sealed class Cashier : IDisposable
    {
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private readonly AtomicLong currentNumber = new AtomicLong();

        public void Dispose()
        {
            semaphore.Dispose();
        }

        public bool Busy => semaphore.CurrentCount == 0;

        public async Task<Ticket> AcquireTicketAsync()
        {
            await semaphore.WaitAsync().ConfigureAwait(false);
            return new Ticket(currentNumber.Read(), this);
        }

        private void Release(long number)
        {
            var result = currentNumber.IfEqualTo(number).Write(number + 1);
            if (result.Success)
            {
                semaphore.Release();
            }
            else
            {
                throw new InvalidOperationException("Ticket is outdated");
            }
        }

        public struct Ticket : IDisposable
        {
            private readonly Cashier cashier;

            public Ticket(long number, Cashier cashier)
            {
                this.cashier = cashier;
                Number = number;
            }

            public long Number { get; }
            public void Dispose() => cashier.Release(Number);
        }
    }
}