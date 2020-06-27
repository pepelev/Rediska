namespace Rediska.Commands.Sets
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utility;

    public sealed class SSCAN : Command<ScanResult<BulkString>>
    {
        private static readonly PlainBulkString name = new PlainBulkString("SSCAN");
        private readonly Key key;
        private readonly Cursor cursor;
        private readonly Match match;
        private readonly ScanCount count;

        public SSCAN(Key key, Cursor cursor, Match match, ScanCount count)
        {
            this.key = key;
            this.cursor = cursor;
            this.match = match;
            this.count = count;
        }

        public SSCAN(Key key, Cursor cursor, Match match)
            : this(key, cursor, match, ScanCount.None)
        {
        }

        public SSCAN(Key key, Cursor cursor, ScanCount count)
            : this(key, cursor, Match.All, count)
        {
        }

        public SSCAN(Key key, Cursor cursor)
            : this(key, cursor, Match.All, ScanCount.None)
        {
        }

        public override DataType Request => new ScanRequest(Prefix, match, count);

        private IReadOnlyList<BulkString> Prefix => new[]
        {
            name,
            key.ToBulkString(),
            cursor.ToBulkString()
        };

        public override Visitor<ScanResult<BulkString>> ResponseStructure => ScanResultVisitor.BulkStringList;
    }
}