namespace Rediska.Commands.Keys
{
    using System;
    using System.Collections.Generic;
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

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString()
        };

        public override Visitor<KeyType> ResponseStructure => responseStructure;

        private static KeyType Parse(string arg)
        {
            return arg switch
            {
                "none" => KeyType.None,
                "string" => KeyType.String,
                "list" => KeyType.List,
                "set" => KeyType.Set,
                "zset" => KeyType.SortedSet,
                "hash" => KeyType.Hash,
                "stream" => KeyType.Stream,
                _ => throw new ArgumentException($"Expected data type or none, but '{arg}' found")
            };
        }
    }
}