namespace Rediska.Commands.Scripting
{
    using System.Collections.Generic;
    using System.Linq;
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

        public override DataType Request => new PlainArray(Query().ToList());
        public override Visitor<DataType> ResponseStructure => Id.Singleton;

        private IEnumerable<BulkString> Query()
        {
            yield return name;
            yield return sha1.ToBulkString();
            yield return keys.Count.ToBulkString();
            foreach (var key in keys)
            {
                yield return key.ToBulkString();
            }

            foreach (var argument in arguments)
            {
                yield return argument;
            }
        }
    }
}