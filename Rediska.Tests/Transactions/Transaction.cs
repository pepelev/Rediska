using System.Threading.Tasks;
using Rediska.Commands;

namespace Rediska.Tests.Transactions
{
    public abstract class Transaction
    {
        public abstract QueuedCommand<T> Enqueue<T>(Command<T> command);
        public abstract Task<bool> ExecuteAsync();
    }
}