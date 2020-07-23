namespace Rediska
{
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using Utils;

    public sealed class SimpleConnectionFactory
    {
        public async Task<Resource<Connection>> CreateAsync(IPEndPoint endPoint)
        {
            var tcp = new TcpClient
            {
                NoDelay = true
            };
            await tcp.ConnectAsync(endPoint.Address, endPoint.Port).ConfigureAwait(false);
            var stream = tcp.GetStream();
            var connection = new SimpleConnection(stream);
            return new Resource<Connection>(connection, tcp);
        }
    }
}