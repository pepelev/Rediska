namespace Rediska.Commands.HyperLogLog
{
    public sealed class PFCOUNT : Command<long>
    {
        private static readonly BulkString name = new BulkString("PFCOUNT");

        private readonly IReadOnlyCollection<Key> keys;

        public PFCOUNT(IReadOnlyCollection<Key> keys)
        {
            this.keys = keys;
        }

        public PFCOUNT(params Key[] keys)
            : this(keys as IReadOnlyCollection<Key>)
        {
        }

        public override DataType Request => new Array(
            new PrefixedCollection<DataType>(
                name,
                new ProjectingCollection<Key, DataType>(
                    keys,
                    key => key.ToBulkString()
                )
            )
        );

        public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;
    }
}