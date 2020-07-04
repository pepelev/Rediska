namespace Rediska.Commands.Hashes
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utility;

    public sealed class HSCAN : Command<ScanResult<HashEntry>>
    {
        private static readonly PlainBulkString name = new PlainBulkString("HSCAN");
        private readonly Key key;
        private readonly Cursor cursor;
        private readonly Match match;
        private readonly ScanCount count;

        public HSCAN(Key key, Cursor cursor, Match match, ScanCount count)
        {
            this.key = key;
            this.cursor = cursor;
            this.match = match;
            this.count = count;
        }

        public HSCAN(Key key, Cursor cursor, Match match)
            : this(key, cursor, match, ScanCount.Default)
        {
        }

        public HSCAN(Key key, Cursor cursor, ScanCount count)
            : this(key, cursor, Match.All, count)
        {
        }

        public HSCAN(Key key, Cursor cursor)
            : this(key, cursor, Match.All, ScanCount.Default)
        {
        }

        public override DataType Request => new ScanRequest(Prefix, match, count);

        private IReadOnlyList<BulkString> Prefix => new[]
        {
            name,
            key.ToBulkString(),
            cursor.ToBulkString()
        };

        public override Visitor<ScanResult<HashEntry>> ResponseStructure => ScanResultVisitor.HashEntryList;
    }
}