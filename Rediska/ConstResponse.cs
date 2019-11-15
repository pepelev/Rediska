namespace Rediska
{
    using System.Threading.Tasks;
    using Protocol;

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