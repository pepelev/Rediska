using System.Threading.Tasks;
using Rediska.Protocol;

namespace Rediska.Tests.Utilities
{
    public sealed class ConstResponse : Connection.Response
    {
        private readonly DataType value;

        public ConstResponse(DataType value)
        {
            this.value = value;
        }

        public override Task<DataType> ReadAsync() => Task.FromResult(value);
    }
}