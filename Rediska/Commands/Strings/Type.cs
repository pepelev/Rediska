namespace Rediska.Commands.Strings
{
    using System;
    using System.Globalization;

    public readonly struct Type
    {
        public Type(TypeKind kind, byte bits)
        {
            if (kind != TypeKind.Signed && kind != TypeKind.Unsigned)
                throw new ArgumentException("Must be signed on unsigned", nameof(kind));

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

        public override string ToString()
        {
            return Kind switch
            {
                TypeKind.Unsigned => $"u{Bits.ToString(CultureInfo.InvariantCulture)}",
                TypeKind.Signed => $"i{Bits.ToString(CultureInfo.InvariantCulture)}",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static Type Signed(byte bits) => new Type(TypeKind.Signed, bits);
        public static Type Unsigned(byte bits) => new Type(TypeKind.Signed, bits);
    }
}