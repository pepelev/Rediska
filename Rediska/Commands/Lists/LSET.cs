namespace Rediska.Commands.Lists
{
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class LSET : Command<None>
    {
        private static readonly PlainBulkString name = new PlainBulkString("LSET");
        private readonly Key key;
        private readonly Index index;
        private readonly BulkString element;

        public LSET(Key key, Index index, BulkString element)
        {
            this.key = key;
            this.index = index;
            this.element = element;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString(),
            index.ToBulkString(),
            element
        );

        public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
    }
}