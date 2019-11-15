namespace Rediska.Commands.Keys
{
    using System;
    using Protocol;
    using Protocol.Visitors;

    public sealed class TYPE : Command<KeyType>
    {
        private static readonly PlainBulkString name = new PlainBulkString("TYPE");
        private static readonly Visitor<KeyType> responseStructure = SimpleStringExpectation.Singleton.Then(Parse);
        private readonly Key key;

        public TYPE(Key key)
        {
            this.key = key;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString()
        );

        public override Visitor<KeyType> ResponseStructure => responseStructure;

        private static KeyType Parse(string arg)
        {
            switch (arg)
            {
                case "none":
                    return KeyType.None;
                case "string":
                    return KeyType.String;
                case "list":
                    return KeyType.List;
                case "set":
                    return KeyType.Set;
                case "zset":
                    return KeyType.SortedSet;
                case "hash":
                    return KeyType.Hash;
                case "stream":
                    return KeyType.Stream;
                default:
                    throw new ArgumentException($"Expected data type or none, but '{arg}' found");
            }
        }
    }
}