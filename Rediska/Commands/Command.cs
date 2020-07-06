namespace Rediska.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using Protocol;
    using Protocol.Visitors;

    public abstract class Command<T>
    {
        public abstract Visitor<T> ResponseStructure { get; }
        public abstract IEnumerable<BulkString> Request(BulkStringFactory factory);
        public override string ToString() => string.Join(" ", Arguments());

        private IEnumerable<string> Arguments() => Request(BulkStringFactory.Plain)
            .Select(
                bulkString =>
                {
                    var text = bulkString.ToString();
                    foreach (var @char in text)
                    {
                        if (char.IsWhiteSpace(@char) || char.IsControl(@char))
                            return $"'{text.Replace("'", "\\'")}'";
                    }

                    return text;
                }
            );
    }
}