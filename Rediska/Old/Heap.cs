namespace Rediska.Old
{
    public sealed class Heap : ArrayPool
    {
        public override byte[] Rent(int minLength)
        {
            return new byte[minLength];
        }

        public override void Release(byte[] array)
        {
        }
    }
}