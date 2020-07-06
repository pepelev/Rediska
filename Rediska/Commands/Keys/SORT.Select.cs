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
            public abstract IEnumerable<BulkString> Arguments(BulkStringFactory factory);

            private sealed class SelectSelf : Select
            {
                public override int BulkStringsPerItem => 1;

                public override IEnumerable<BulkString> Arguments(BulkStringFactory factory) =>
                    Enumerable.Empty<BulkString>();
            }

            public sealed class Patterns : Select
            {
                private static readonly PlainBulkString get = new PlainBulkString("GET");
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

                public override IEnumerable<BulkString> Arguments(BulkStringFactory factory)
                {
                    foreach (var pattern in patterns)
                    {
                        yield return get;
                        yield return factory.Utf8(pattern);
                    }
                }
            }
        }
    }
}