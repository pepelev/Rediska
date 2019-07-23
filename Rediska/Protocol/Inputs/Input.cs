namespace Rediska.Protocol.Inputs
{
    public abstract class Input
    {
        public abstract Magic ReadMagic();
        public abstract string ReadSimpleString();
        public abstract long ReadInteger();
        public abstract BulkString ReadBulkString();
    }
}