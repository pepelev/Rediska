using System;
using System.Threading.Tasks;
using Rediska.Commands.Strings;

namespace Rediska.Tests.Transactions
{
    public static class ConnectionExtensions
    {
        public static Task<T> PrepareTransaction<T>(
            this Connection connection,
            Func<Transaction, T> payload)
        {
            return Task.FromResult(default(T));
        }
    }

    public abstract class QueuedCommand<T>
    {
        public abstract T Result { get; }
    }

    public class Demonstration
    {
        private readonly Connection connection;

        public void A()
        {
            connection.PrepareTransaction(transaction =>
                {
                    return new
                    {
                        GetA = transaction.Enqueue(new GET("key-1")),
                        GetB = transaction.Enqueue(new GET("key-2"))
                    };
                }
            );
        }
    }
}