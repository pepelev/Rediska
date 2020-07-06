namespace Rediska
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Commands;
    using Protocol;

    public static class ConnectionExtensions
    {
        public static async Task<T> ExecuteAsync<T>(this Connection connection, Command<T> command)
        {
            var request = command.Request(BulkStringFactory.Plain) switch
            {
                IReadOnlyList<BulkString> list => new PlainArray(list),
                { } sequence => new PlainArray(sequence.ToList()),
                null => throw new ArgumentException("Must return non-null for .Request()", nameof(command))
            };
            using var response = await connection.SendAsync(request).ConfigureAwait(false);
            var content = await response.Value.ReadAsync().ConfigureAwait(false);
            return content.Accept(command.ResponseStructure);
        }
    }
}