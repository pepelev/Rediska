namespace Rediska.Tests.Commands.Sets
{
    using System.Threading.Tasks;

    public abstract class ConnectionFixture
    {
        public abstract KeyFixture Keys { get; }
        public abstract Task<Connection> SetUpAsync();
        public abstract Task TearDownAsync();
    }
}