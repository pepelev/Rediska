namespace Rediska.Commands.SortedSets
{
    using System;
    using System.IO;
    using System.Text;
    using Protocol;

    public readonly struct LexBound
    {
        private static readonly PlainBulkString positiveInfinity = new PlainBulkString("+");
        private static readonly PlainBulkString negativeInfinity = new PlainBulkString("-");
        public static LexBound PositiveInfinity => new LexBound(Kind.PositiveInfinity, positiveInfinity);
        public static LexBound NegativeInfinity => new LexBound(Kind.NegativeInfinity, negativeInfinity);
        private readonly Kind kind;
        private readonly BulkString value;

        private LexBound(Kind kind, BulkString value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (value.IsNull)
                throw new ArgumentException("Must not be null bulk string", nameof(value));

            this.kind = kind;
            this.value = value;
        }

        public BulkString ToBulkString() => kind switch
        {
            Kind.NegativeInfinity => negativeInfinity,
            Kind.PositiveInfinity => positiveInfinity,
            Kind.Inclusive => PrefixWith(Bounds.InclusiveSign),
            _ => PrefixWith(Bounds.ExclusiveSign)
        };

        private BulkString PrefixWith(string prefix)
        {
            using var stream = new MemoryStream(Math.Max(1, (int) (value.Length + prefix.Length)));
            using var writer = new StreamWriter(stream, Encoding.UTF8);
            writer.Write(prefix);
            value.WriteContent(stream);
            return new PlainBulkString(stream.ToArray());
        }

        public override string ToString() => ToBulkString().ToString();

        public static bool IsVoid(in LexBound min, in LexBound max) => (min.kind, max.kind) switch
        {
            (Kind.NegativeInfinity, _) => false,
            (_, Kind.PositiveInfinity) => false,
            (Kind.Inclusive, Kind.Inclusive) => Compare(min.value, max.value) <= 0,
            (Kind.Exclusive, Kind.Exclusive) => Compare(min.value, max.value) < 0,
            (Kind.Exclusive, Kind.Inclusive) => Compare(min.value, max.value) < 0,
            (Kind.Inclusive, Kind.Exclusive) => Compare(min.value, max.value) < 0,
            _ => true
        };

        private static int Compare(BulkString a, BulkString b)
        {
            using var streamA = new MemoryStream();
            using var streamB = new MemoryStream();
            a.WriteContent(streamA);
            b.WriteContent(streamB);

            var bound = Math.Min(streamA.Length, streamB.Length);
            var arrayA = streamA.GetBuffer();
            var arrayB = streamB.GetBuffer();

            for (var i = 0; i < bound; i++)
            {
                if (arrayA[i] > arrayB[i])
                    return 1;
                if (arrayA[i] < arrayB[i])
                    return -1;
            }

            return streamA.Length.CompareTo(streamB.Length);
        }
    }
}