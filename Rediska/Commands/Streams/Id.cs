namespace Rediska.Commands.Streams
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using Protocol;

    public readonly struct Id : IEquatable<Id>, IComparable<Id>
    {
        private static readonly Regex pattern = new Regex(
            @"^\s*(?<High>[0-9]+)-(?<Low>[0-9]+)\s*$",
            RegexOptions.CultureInvariant | RegexOptions.Compiled
        );

        public static Id Minimum => new Id(0, 0);
        public static Id Maximum => new Id(ulong.MaxValue, ulong.MaxValue);

        public Id(ulong high, ulong low)
        {
            High = high;
            Low = low;
        }

        public ulong High { get; }
        public ulong Low { get; }
        public UnixMillisecondsTimestamp Timestamp => new UnixMillisecondsTimestamp(checked((long) High));
        public ulong SequenceNumber => Low;

        public int CompareTo(Id other)
        {
            var highComparison = High.CompareTo(other.High);
            if (highComparison != 0)
                return highComparison;

            return Low.CompareTo(other.Low);
        }

        public bool Equals(Id other) => High == other.High && Low == other.Low;
        public static bool operator ==(Id left, Id right) => left.Equals(right);
        public static bool operator >(Id left, Id right) => left.CompareTo(right) > 0;
        public static bool operator >=(Id left, Id right) => left.CompareTo(right) >= 0;
        public static bool operator !=(Id left, Id right) => !left.Equals(right);
        public static bool operator <(Id left, Id right) => left.CompareTo(right) < 0;
        public static bool operator <=(Id left, Id right) => left.CompareTo(right) <= 0;

        public BulkString ToBulkString(BulkStringFactory factory, Print mode)
        {
            BulkString Full(Id id) => factory.Utf8(id.ToString());

            return mode switch
            {
                Print.Full => Full(this),
                Print.SkipMinimalLow when Low == 0 => factory.Create(High),
                Print.SkipMinimalLow => Full(this),
                Print.SkipMaximalLow when Low == ulong.MaxValue => factory.Create(High),
                Print.SkipMaximalLow => Full(this),
                _ => throw WrongMode(mode)
            };
        }

        private static ArgumentException WrongMode(Print mode)
        {
            var message = $"Must be Full, AllowSkipMinimalLow or AllowSkipMaximalLow, but {mode} is found";
            return new ArgumentException(message);
        }

        [Pure]
        public Id Next()
        {
            if (Low < ulong.MaxValue)
                return new Id(High, Low + 1);

            if (High < ulong.MaxValue)
                return MinFor(High + 1);

            throw new InvalidOperationException("Id is Maximum");
        }

        [Pure]
        public Id Previous()
        {
            if (Low > 0)
                return new Id(High, Low - 1);

            if (High > 0)
                return MaxFor(High - 1);

            throw new InvalidOperationException("Id is Minimum");
        }

        public override bool Equals(object obj) => obj is Id other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                return (High.GetHashCode() * 397) ^ Low.GetHashCode();
            }
        }

        public string ToString(Print mode)
        {
            static string Render(ulong part) => part.ToString(CultureInfo.InvariantCulture);

            static string Full(Id id) => $"{Render(id.High)}-{Render(id.Low)}";

            return mode switch
            {
                Print.Full => Full(this),
                Print.SkipMinimalLow when Low == 0 => Render(High),
                Print.SkipMinimalLow => Full(this),
                Print.SkipMaximalLow when Low == ulong.MaxValue => Render(High),
                Print.SkipMaximalLow => Full(this),
                _ => throw WrongMode(mode)
            };
        }

        public override string ToString() => ToString(Print.Full);

        public static bool TryParse(string input, out Id result)
        {
            if (pattern.Match(input) is {Success: true} match)
            {
                var highSuccess = ulong.TryParse(match.Groups["High"].Value, out var high);
                var lowSuccess = ulong.TryParse(match.Groups["Low"].Value, out var low);
                if (highSuccess && lowSuccess)
                {
                    result = new Id(high, low);
                    return true;
                }
            }

            result = Minimum;
            return false;
        }

        public static Id Parse(string input)
        {
            if (TryParse(input, out var result))
                return result;

            throw new FormatException($"Input {input} could not be parsed as Id");
        }

        public static Id MinFor(ulong high) => new Id(high, ulong.MinValue);
        public static Id MaxFor(ulong high) => new Id(high, ulong.MaxValue);

        public static Id MinFor(DateTime dateTime) => MinFor(
            ToUnixMillisecondsTimestamp(dateTime)
        );

        public static Id MaxFor(DateTime dateTime) => MaxFor(
            ToUnixMillisecondsTimestamp(dateTime)
        );

        private static ulong ToUnixMillisecondsTimestamp(DateTime dateTime) =>
            checked((ulong) (dateTime.Ticks / TimeSpan.TicksPerMillisecond));

        public enum Print : byte
        {
            Full = 0,
            SkipMinimalLow = 1,
            SkipMaximalLow = 2
        }
    }
}