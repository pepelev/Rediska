namespace Rediska.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using Protocol;

    public abstract class Match
    {
        public static Match All { get; } = new AllMatch();
        public static Match Pattern(string pattern) => new PatternMatch(pattern);
        public abstract IEnumerable<BulkString> Arguments(BulkStringFactory factory);

        private sealed class AllMatch : Match
        {
            public override string ToString() => "*";
            public override IEnumerable<BulkString> Arguments(BulkStringFactory factory) => Enumerable.Empty<BulkString>();
        }

        private sealed class PatternMatch : Match
        {
            private static readonly PlainBulkString match = new PlainBulkString("MATCH");
            private readonly string pattern;

            public PatternMatch(string pattern)
            {
                this.pattern = pattern;
            }

            public override IEnumerable<BulkString> Arguments(BulkStringFactory factory)
            {
                yield return match;
                yield return factory.Utf8(pattern);
            }
        }
    }
}