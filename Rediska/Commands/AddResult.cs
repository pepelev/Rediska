namespace Rediska.Commands
{
    public readonly struct AddResult
    {
        public AddResult(long added)
        {
            Added = added;
        }

        public long Added { get; }
    }
}