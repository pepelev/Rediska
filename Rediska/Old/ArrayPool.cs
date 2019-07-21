namespace Rediska.Old
{
    public abstract class ArrayPool
    {
        public abstract byte[] Rent(int minLength);
        public abstract void Release(byte[] array);
    }
}