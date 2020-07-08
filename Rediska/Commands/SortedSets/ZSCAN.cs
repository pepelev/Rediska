namespace Rediska.Commands.SortedSets
{
    using System.Collections.Generic;
    using System.Linq;
    using Protocol;
    using Protocol.Visitors;

    public sealed class ZSCAN : Command<ScanResult<(BulkString Member, double Score)>>
    {
        private static readonly PlainBulkString name = new PlainBulkString("ZSCAN");
        private readonly Key key;
        private readonly Cursor cursor;
        private readonly Match match;
        private readonly ScanCount count;

        public ZSCAN(Key key, Cursor cursor)
            : this(key, cursor, Match.All, ScanCount.Default)
        {
        }

        public ZSCAN(Key key, Cursor cursor, Match match)
            : this(key, cursor, match, ScanCount.Default)
        {
        }

        public ZSCAN(Key key, Cursor cursor, ScanCount count)
            : this(key, cursor, Match.All, count)
        {
        }

        public ZSCAN(Key key, Cursor cursor, Match match, ScanCount count)
        {
            this.key = key;
            this.cursor = cursor;
            this.match = match;
            this.count = count;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => Prefix(factory)
            .Concat(match.Arguments(factory))
            .Concat(count.Arguments(factory));

        public override Visitor<ScanResult<(BulkString Member, double Score)>> ResponseStructure =>
            ScanResultVisitor.SortedSetEntryList;

        private IEnumerable<BulkString> Prefix(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory),
            cursor.ToBulkString(factory)
        };
    }
}