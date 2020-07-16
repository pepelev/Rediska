namespace Rediska.Commands.Scripting
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class EVALSHA : Command<DataType>
    {
        private static readonly PlainBulkString name = new PlainBulkString("EVALSHA");
        private readonly Sha1 sha1;
        private readonly IReadOnlyList<Key> keys;
        private readonly IReadOnlyList<BulkString> arguments;

        public EVALSHA(Sha1 sha1, IReadOnlyList<Key> keys, IReadOnlyList<BulkString> arguments)
        {
            this.sha1 = sha1;
            this.keys = keys;
            this.arguments = arguments;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory)
        {
            yield return name;
            yield return factory.Create(sha1);
            yield return factory.Create(keys.Count);
            foreach (var key in keys)
            {
                yield return key.ToBulkString(factory);
            }

            foreach (var argument in arguments)
            {
                yield return argument;
            }
        }

        public override Visitor<DataType> ResponseStructure => Id.Singleton;
    }
}