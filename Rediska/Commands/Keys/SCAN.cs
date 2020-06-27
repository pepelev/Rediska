namespace Rediska.Commands.Keys
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utility;

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
            : this(cursor, match, ScanCount.None)
        {
        }

        public SCAN(Cursor cursor, ScanCount count)
            : this(cursor, Match.All, count)
        {
        }

        public SCAN(Cursor cursor)
            : this(cursor, Match.All, ScanCount.None)
        {
        }

        public override DataType Request => new ScanRequest(Prefix, match, count);

        private IReadOnlyList<BulkString> Prefix => new[]
        {
            name,
            cursor.ToBulkString()
        };

        public override Visitor<ScanResult<Key>> ResponseStructure => ScanResultVisitor.KeyList;
    }
}