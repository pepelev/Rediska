namespace Rediska.Commands.Keys
{
    using System.Collections.Generic;
    using System.Linq;
    using Protocol;
    using Protocol.Visitors;

    public sealed class SCAN : Command<ScanResult<Key>>
    {
        private static readonly PlainBulkString name = new PlainBulkString("SCAN");
        private readonly Cursor cursor;
        private readonly Match match;
        private readonly ScanCount count;

        public SCAN(Cursor cursor, Match match, ScanCount count)
        {
            this.cursor = cursor;
            this.match = match;
            this.count = count;
        }

        public SCAN(Cursor cursor, Match match)
            : this(cursor, match, ScanCount.Default)
        {
        }

        public SCAN(Cursor cursor, ScanCount count)
            : this(cursor, Match.All, count)
        {
        }

        public SCAN(Cursor cursor)
            : this(cursor, Match.All, ScanCount.Default)
        {
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => Prefix(factory)
            .Concat(match.Arguments(factory))
            .Concat(count.Arguments(factory));

        public override Visitor<ScanResult<Key>> ResponseStructure => ScanResultVisitor.KeyList;

        private IEnumerable<BulkString> Prefix(BulkStringFactory factory) => new[]
        {
            name,
            cursor.ToBulkString(factory)
        };
    }
}