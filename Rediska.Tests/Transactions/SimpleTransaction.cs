using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rediska.Commands;
using Rediska.Commands.Transactions;
using Rediska.Protocol;
using Rediska.Utils;

namespace Rediska.Tests.Transactions
{
    public sealed class SimpleTransaction : Transaction
    {
        private readonly BulkConnection connection;
        private readonly List<(IQueuedCommand, Resource<Connection.Response>)> commands = new List<(IQueuedCommand, Resource<Connection.Response>)>();

        public SimpleTransaction(BulkConnection connection)
        {
            this.connection = connection;
            var (command, response) = EnqueueInternal(MULTI.Singleton);
        }

        public override QueuedCommand<T> Enqueue<T>(Command<T> command)
        {
            var pair = EnqueueInternal(command);
            commands.Add(pair);
            var (queuedCommand, _) = pair;
            return queuedCommand;
        }

        private (SimpleQueuedCommand<T>, Resource<Connection.Response>) EnqueueInternal<T>(Command<T> command)
        {
            var queuedCommand = new SimpleQueuedCommand<T>(command);
            var response = connection.Send(command.Request);
            return (queuedCommand, response);
        }

        public override async Task<bool> ExecuteAsync()
        {
            var (command, response) = EnqueueInternal(EXEC.Singleton);
            using (response)
            {
                await connection.FlushAsync().ConfigureAwait(false);
                var transactionResult = await response.Value.ReadAsync().ConfigureAwait(false);
                var array = transactionResult.Accept(EXEC.Singleton.ResponseStructure);
                for (var i = 0; i < commands.Count; i++)
                {
                    commands[i].Item1.Set(array[i]);
                    commands[i].Item2.Dispose();
                }
            }

            commands.Clear();
            return true;
        }

        private sealed class SimpleQueuedCommand<T> : QueuedCommand<T>, IQueuedCommand
        {
            private readonly Command<T> command;
            private DataType response;

            public SimpleQueuedCommand(Command<T> command)
            {
                this.command = command;
            }

            public override T Result => ResponseSet
                ? response.Accept(command.ResponseStructure)
                : throw new InvalidOperationException("No reply received yet");

            private bool ResponseSet => !ReferenceEquals(response, null);

            public override string ToString()
            {
                var responseText = response?.ToString() ?? "Pending...";
                return string.Join(
                    Environment.NewLine,
                    $"-> {command}",
                    $"<- {responseText}"
                );
            }

            public void Set(DataType response)
            {
                if (ResponseSet)
                    throw new InvalidOperationException();

                this.response = response;
            }
        }

        private interface IQueuedCommand
        {
            void Set(DataType response);
        }
    }
}