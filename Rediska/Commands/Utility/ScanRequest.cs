namespace Rediska.Commands.Utility
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Protocol;

    internal sealed class ScanRequest : IEnumerable<BulkString>
    {
        private readonly IReadOnlyList<BulkString> prefix;
        private readonly Match match;
        private readonly ScanCount count;

        public ScanRequest(IReadOnlyList<BulkString> prefix, Match match, ScanCount count)
        {
            this.prefix = prefix;
            this.match = match;
            this.count = count;
        }

        public IEnumerator<BulkString> GetEnumerator() => prefix.Concat(match).Concat(count).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}