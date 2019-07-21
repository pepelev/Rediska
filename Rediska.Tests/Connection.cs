using System.Threading.Tasks;

namespace Rediska.Tests
{
    public abstract class Connection
    {
        public abstract Task<Resource<Response>> SendAsync(Protocol.Requests.DataType command);

        public abstract class Response
        {
            public abstract Task<Protocol.Responses.DataType> ReadAsync();
        }
    }
}