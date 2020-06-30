namespace Rediska
{
    using System.Threading.Tasks;
    using Commands;

    public static class ConnectionExtensions
    {
        public static async Task<T> ExecuteAsync<T>(this Connection connection, Command<T> command)
        {
            using var response = await connection.SendAsync(command.Request).ConfigureAwait(false);
            var content = await response.Value.ReadAsync().ConfigureAwait(false);
            return content.Accept(command.ResponseStructure);
        }
    }
}