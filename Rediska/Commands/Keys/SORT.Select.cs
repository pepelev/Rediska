namespace Rediska.Commands.Keys
{
    using System.Collections.Generic;
    using System.Linq;
    using Protocol;

    public sealed partial class SORT
    {
        public abstract class Select
        {
            public static Select Self { get; } = new SelectSelf();
            public abstract int BulkStringsPerItem { get; }
            public abstract IEnumerable<BulkString> Query();

            private sealed class SelectSelf : Select
            {
                public override int BulkStringsPerItem => 1;
                public override IEnumerable<BulkString> Query() => Enumerable.Empty<BulkString>();
            }

            public sealed class Patterns : Select
            {
                private readonly IReadOnlyList<string> patterns;

                public Patterns(params string[] patterns)
                    : this(patterns as IReadOnlyList<string>)
                {
                }

                public Patterns(IReadOnlyList<string> patterns)
                {
                    this.patterns = patterns;
                }

                public override int BulkStringsPerItem => patterns.Count;

                public override IEnumerable<BulkString> Query()
                {
                    foreach (var pattern in patterns)
                    {
                        yield return new PlainBulkString("GET");
                        yield return new PlainBulkString(pattern);
                    }
                }
            }
        }
    }
}