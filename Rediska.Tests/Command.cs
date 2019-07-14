namespace Rediska.Tests
{
    public abstract class Command<T>
    {
        public abstract void Write(Output request);
        public abstract T Read(Input response);
    }
}