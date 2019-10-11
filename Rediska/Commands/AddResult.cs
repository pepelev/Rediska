namespace Rediska.Commands
{
    public struct AddResult
    {
        public AddResult(long added)
        {
            Added = added;
        }

        public long Added { get; }
    }
}