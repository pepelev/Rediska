namespace Rediska
{
    using Commands;

    public abstract partial class Key
    {
        public static implicit operator Key(string value) => new Utf8(value);
        public static implicit operator Key(byte[] value) => new Raw(value);
        public abstract byte[] ToBytes();
        public abstract Protocol.BulkString ToBulkString(BulkStringFactory factory);
    }
}