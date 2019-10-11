namespace Rediska.Commands.Sets
{
    using System.Collections.Generic;
    using System.Globalization;
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

        public override DataType Request
        {
            get
            {
                if (match.Count == 0 && count.Count == 0)
                    return new PlainArray(Prefix);

                if (match.Count == 0)
                {
                    return new PlainArray(
                        new ConcatList<DataType>(
                            Prefix,
                            count
                        )
                    );
                }

                if (count.Count == 0)
                {
                    return new PlainArray(
                        new ConcatList<DataType>(
                            Prefix,
                            match
                        )
                    );
                }

                return new PlainArray(
                    new ConcatList<DataType>(
                        Prefix,
                        new ConcatList<DataType>(
                            match,
                            count
                        )
                    )
                );
            }
        }

        private IReadOnlyList<DataType> Prefix => new DataType[]
        {
            name,
            key.ToBulkString(),
            Cursor
        };

        private PlainBulkString Cursor => new PlainBulkString(cursor.Value.ToString(CultureInfo.InvariantCulture));
        public override Visitor<ScanResult> ResponseStructure => ScanResultVisitor.Singleton;
    }
}