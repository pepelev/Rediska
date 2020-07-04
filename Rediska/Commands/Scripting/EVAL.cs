namespace Rediska.Commands.Scripting
{
    using System.Collections.Generic;
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

        public override IEnumerable<BulkString> Request(BulkStringFactory factory)
        {
            yield return name;
            yield return factory.Utf8(script);
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

        public override Visitor<DataType> ResponseStructure => Id.Singleton;
    }
}