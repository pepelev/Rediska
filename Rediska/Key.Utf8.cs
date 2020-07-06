namespace Rediska
{
    using System.Text;
    using Commands;

    public abstract partial class Key
    {
        public sealed class Utf8 : Key
        {
            private readonly string value;

            public Utf8(string value)
            {
                this.value = value;
            }

            public override byte[] ToBytes() => Encoding.UTF8.GetBytes(value);
            public override Protocol.BulkString ToBulkString(BulkStringFactory factory) => factory.Utf8(value);
            public override string ToString() => value;
        }
    }
}