namespace Rediska.Commands.Sets
{
    using System.Collections.Generic;
    using System.Linq;
    using Protocol;
    using Protocol.Visitors;

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
            : this(key, cursor, match, ScanCount.Default)
        {
        }

        public SSCAN(Key key, Cursor cursor, ScanCount count)
            : this(key, cursor, Match.All, count)
        {
        }

        public SSCAN(Key key, Cursor cursor)
            : this(key, cursor, Match.All, ScanCount.Default)
        {
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => Prefix(factory)
            .Concat(match.Arguments(factory))
            .Concat(count.Arguments(factory));

        public override Visitor<ScanResult<BulkString>> ResponseStructure => ScanResultVisitor.BulkStringList;

        private IEnumerable<BulkString> Prefix(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory),
            cursor.ToBulkString(factory)
        };
    }
}