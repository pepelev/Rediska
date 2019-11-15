namespace Rediska
{
    using System.Threading.Tasks;
    using Protocol;
    using Utils;

    public abstract class Connection
    {
        // todo поддержать отмену
        public abstract Task<Resource<Response>> SendAsync(DataType command);

        public abstract class Response
        {
            public abstract Task<DataType> ReadAsync();
        }
    }
}