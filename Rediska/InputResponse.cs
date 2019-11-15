using System.Threading.Tasks;
using Rediska.Protocol;
using Rediska.Protocol.Inputs;

namespace Rediska
{
    public sealed class InputResponse : Connection.Response
    {
        private readonly Input input;

        public InputResponse(Input input)
        {
            this.input = input;
        }

        public override Task<DataType> ReadAsync()
        {
            return Task.FromResult(
                input.Read()
            );
        }
    }
}