namespace Rediska.Commands.Sets
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class SSCAN : Command<ScanResult>
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

        public override DataType Request => (match.Count, count.Count) switch
        {
            (0, 0) => new PlainArray(Prefix),
            (0, _) => new PlainArray(
                new ConcatList<DataType>(
                    Prefix,
                    count
                )
            ),
            (_, 0) => new PlainArray(
                new ConcatList<DataType>(
                    Prefix,
                    match
                )
            ),
            _ => new PlainArray(
                new ConcatList<DataType>(
                    Prefix,
                    new ConcatList<DataType>(
                        match,
                        count
                    )
                )
            )
        };

    private IReadOnlyList<DataType> Prefix => new DataType[]
        {
            name,
            key.ToBulkString(),
            cursor.ToBulkString()
        };

        public override Visitor<ScanResult> ResponseStructure => ScanResultVisitor.Singleton;
    }
}