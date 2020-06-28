namespace Rediska.Commands.Geo
{
    public readonly struct StoreResponse
    {
        public StoreResponse(Key target, long itemsSaved)
        {
            ItemsSaved = itemsSaved;
            Target = target;
        }

        public Key Target { get; }
        public long ItemsSaved { get; }
        public bool TargetOverwritten => ItemsSaved > 0;
    }
}