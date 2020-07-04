namespace Rediska.Commands.Scripting
{
    using System.Collections.Generic;
    using System.Linq;
    using Protocol;
    using Protocol.Visitors;

    public sealed class EVAL : Command<DataType>
    {
        private static readonly PlainBulkString name = new PlainBulkString("EVAL");
        private readonly string script;
        private readonly IReadOnlyList<Key> keys;
        private readonly IReadOnlyList<BulkString> arguments;

        public EVAL(string script, IReadOnlyList<Key> keys, IReadOnlyList<BulkString> arguments)
        {
            this.script = script;
            this.keys = keys;
            this.arguments = arguments;
        }

        public override DataType Request => new PlainArray(Query().ToList());
        public override Visitor<DataType> ResponseStructure => Id.Singleton;

        private IEnumerable<BulkString> Query()
        {
            yield return name;
            yield return new PlainBulkString(script);
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