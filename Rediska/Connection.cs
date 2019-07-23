using System.Threading.Tasks;
using Rediska.Protocol;
using Rediska.Utils;

namespace Rediska
{
    public abstract class Connection
    {
        public abstract Task<Resource<Response>> SendAsync(DataType command);

        public abstract class Response
        {
            public abstract Task<DataType> ReadAsync();
        }
    }
}