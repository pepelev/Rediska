namespace Rediska.Commands.Utility
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Outputs;
    using Protocol.Visitors;
    using Utils;

    internal sealed class ScanRequest : DataType
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

        private DataType Content => (match.Count, count.Count) switch
        {
            (0, 0) => new PlainArray(prefix),
            (0, _) => new PlainArray(
                new ConcatList<DataType>(
                    prefix,
                    count
                )
            ),
            (_, 0) => new PlainArray(
                new ConcatList<DataType>(
                    prefix,
                    match
                )
            ),
            _ => new PlainArray(
                new ConcatList<DataType>(
                    prefix,
                    new ConcatList<DataType>(
                        match,
                        count
                    )
                )
            )
        };

        public override T Accept<T>(Visitor<T> visitor) => Content.Accept(visitor);
        public override void Write(Output output) => Content.Write(output);
    }
}