namespace Rediska.Commands.Hashes
{
    using System.Collections;
    using System.Collections.Generic;
    using Protocol;

    internal sealed class SetRequest : IReadOnlyList<BulkString>
    {
        private readonly BulkString commandName;
        private readonly Key key;

        public SetRequest(
            BulkString commandName,
            Key key,
            IReadOnlyList<(Key Field, BulkString Value)> pairs)
        {
            this.commandName = commandName;
            this.key = key;
            this.pairs = pairs;
        }

        private readonly IReadOnlyList<(Key Key, BulkString Value)> pairs;

        public IEnumerator<BulkString> GetEnumerator()
        {
            yield return commandName;
            yield return key.ToBulkString();
            foreach (var (field, value) in pairs)
            {
                yield return field.ToBulkString();
                yield return value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public int Count => 2 + pairs.Count;

        public BulkString this[int index] => index switch
        {
            0 => commandName,
            1 => key.ToBulkString(),
            var odd when odd % 2 == 1 => pairs[index / 2 - 1].Key.ToBulkString(),
            _ => pairs[index / 2 - 1].Value
        };
    }
}