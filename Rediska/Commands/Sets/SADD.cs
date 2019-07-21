namespace Rediska.Commands.Sets
{
    public sealed class SADD : Command<long>
    {
        private readonly Key key;
        private readonly IReadOnlyCollection<BulkString> members;

        public SADD(Key key, IReadOnlyCollection<BulkString> members)
        {
            this.key = key;
            this.members = members;
        }

        public override DataType Request => new Array(
            new ConcatCollection<DataType>(
                new DataType[]
                {
                    new BulkString("SADD"),
                    key.ToBulkString()
                },
                members
            )
        );

        public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;
    }
}