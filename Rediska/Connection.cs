namespace Rediska
{
    using System.Threading;
    using System.Threading.Tasks;
    using Protocol;

    public abstract class Connection
    {
        // todo поддержать отмену
        public abstract Task<Response> SendAsync(DataType command, CancellationToken token);

        public abstract class Response
        {
            public abstract Task<DataType> ReadAsync();
        }
    }
}