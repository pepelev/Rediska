namespace Rediska
{
    using System.Text;
    using Commands;

    public abstract partial class Key
    {
        public sealed class Raw : Key
        {
            private readonly byte[] value;

            public Raw(byte[] value)
            {
                this.value = value;
            }

            public override byte[] ToBytes() => value;
            public override Protocol.BulkString ToBulkString(BulkStringFactory factory) => factory.Create(value);
            public override string ToString() => Encoding.UTF8.GetString(value);
        }
    }
}