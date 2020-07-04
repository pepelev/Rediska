namespace Rediska.Commands.Keys
{
    using System.Collections.Generic;
    using System.Linq;
    using Protocol;

    public sealed partial class SORT
    {
        public abstract class By
        {
            private static readonly PlainBulkString @by = new PlainBulkString("BY");
            public abstract IEnumerable<BulkString> Query();
            public static By Self { get; } = new BySelf();
            public static By Nothing { get; } = new ByNothing();

            public sealed class Pattern : By
            {
                private readonly string pattern;

                public Pattern(string pattern)
                {
                    this.pattern = pattern;
                }

                public override IEnumerable<BulkString> Query()
                {
                    yield return @by;
                    yield return new PlainBulkString(pattern);
                }
            }

            private sealed class ByNothing : By
            {
                private static readonly PlainBulkString nosort = new PlainBulkString("nosort");

                public override IEnumerable<BulkString> Query()
                {
                    yield return @by;
                    yield return nosort;
                }
            }

            private sealed class BySelf : By
            {
                public override IEnumerable<BulkString> Query() => Enumerable.Empty<BulkString>();
            }
        }
    }
}