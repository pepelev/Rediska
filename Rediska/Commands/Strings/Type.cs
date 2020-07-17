namespace Rediska.Commands.Strings
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using Protocol;

    public readonly struct Type
    {
        public Type(TypeKind kind, byte bits)
        {
            if (kind != TypeKind.Signed && kind != TypeKind.Unsigned)
                throw new ArgumentException("Must be signed or unsigned", nameof(kind));

            if (bits == 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(bits),
                    bits,
                    "Bits count must be positive"
                );
            }

            // check for 0 bits
            if (kind == TypeKind.Signed && bits > 64)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(bits),
                    bits,
                    "Signed numbers must be at most 64 bits wide"
                );
            }

            if (kind == TypeKind.Unsigned && bits > 63)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(bits),
                    bits,
                    "Unsigned numbers must be at most 63 bits wide"
                );
            }

            Kind = kind;
            Bits = bits;
        }

        public TypeKind Kind { get; }
        public byte Bits { get; }

        [Pure]
        public (long Left, long Right) Range
        {
            get
            {
                return (Kind, Bits) switch
                {
                    (TypeKind.Signed, 64) => (long.MinValue, long.MaxValue),
                    (TypeKind.Signed, _) => (-(1L << (Bits - 1)), (long) ~(ulong.MaxValue << (Bits - 1))),
                    _ => (0, ~(long.MaxValue << Bits))
                };
            }
        }

        [Pure]
        public bool Fit(long value)
        {
            var (left, right) = Range;
            return left <= value && value <= right;
        }

        public override string ToString()
        {
            return Kind switch
            {
                TypeKind.Unsigned => $"u{Bits.ToString(CultureInfo.InvariantCulture)}",
                TypeKind.Signed => $"i{Bits.ToString(CultureInfo.InvariantCulture)}",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public BulkString ToBulkString(BulkStringFactory factory) => factory.Utf8(ToString());
        public static Type Signed(byte bits) => new Type(TypeKind.Signed, bits);
        public static Type Unsigned(byte bits) => new Type(TypeKind.Unsigned, bits);
    }
}