namespace Rediska.Commands.Scripting
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Protocol;

    public readonly struct Sha1 : IEquatable<Sha1>
    {
        private static readonly byte[] defaultContent = new byte[20];
        private readonly byte[] content;
        public const int Size = 20;
        public const int HexSize = 40;

        public Sha1(byte[] content)
        {
            if (content.Length != Size)
                throw new ArgumentException("Must contain 20 bytes", nameof(content));

            this.content = content;
        }

        private byte[] Content => content ?? defaultContent;
        public byte[] ToBytes() => Content.ToArray();
        public BulkString ToBulkString() => new PlainBulkString(ToString());

        public bool Equals(Sha1 other)
        {
            if (ReferenceEquals(Content, other.Content))
                return true;

            for (var i = 0; i < 20; i++)
            {
                if (Content[i] != other.Content[i])
                    return false;
            }

            return true;
        }

        public override bool Equals(object obj) => obj is Sha1 other && Equals(other);

        public override int GetHashCode()
        {
            var result = 0;
            foreach (var @byte in Content)
            {
                result = (result * 319) ^ @byte;
            }

            return result;
        }

        public override string ToString()
        {
            var builder = new StringBuilder(40);
            foreach (var @byte in Content)
            {
                var high = @byte / 16;
                var low = @byte % 16;

                builder.Append(ToChar(high));
                builder.Append(ToChar(low));

                static char ToChar(int part)
                {
                    return part < 10
                        ? (char) ('0' + part)
                        : (char) ('a' - 10 + part);
                }
            }

            return builder.ToString();
        }

        public static bool operator ==(Sha1 left, Sha1 right) => left.Equals(right);
        public static bool operator !=(Sha1 left, Sha1 right) => !left.Equals(right);

        public static Sha1 Parse(string hex)
        {
            if (TryParse(hex, out var result))
            {
                return result;
            }

            throw new FormatException($"{hex} could not be parsed as Sha1");
        }

        public static bool TryParse(string hex, out Sha1 result)
        {
            if (hex == null)
            {
                result = default;
                return false;
            }

            if (hex.Length < HexSize)
            {
                result = default;
                return false;
            }

            var trimmedHex = hex.Trim();
            if (trimmedHex.Length != HexSize)
            {
                result = default;
                return false;
            }

            var bytes = new byte[Size];
            for (var i = 0; i < Size; i++)
            {
                var high = char.ToLowerInvariant(trimmedHex[i * 2]);
                var low = char.ToLowerInvariant(trimmedHex[i * 2 + 1]);

                if ('0' <= high && high <= '9')
                {
                    bytes[i] = (byte) ((high - '0') * 16);
                }
                else if ('a' <= high && high <= 'f')
                {
                    bytes[i] = (byte) ((high - 'a' + 10) * 16);
                }
                else
                {
                    result = default;
                    return false;
                }

                if ('0' <= low && low <= '9')
                {
                    bytes[i] += (byte) (low - '0');
                }
                else if ('a' <= low && low <= 'f')
                {
                    bytes[i] += (byte) (low - 'a' + 10);
                }
                else
                {
                    result = default;
                    return false;
                }
            }

            result = new Sha1(bytes);
            return true;
        }

        public static Sha1 Create(byte[] bytes)
        {
            using var hash = SHA1.Create();
            var hashBytes = hash.ComputeHash(bytes);
            return new Sha1(hashBytes);
        }

        public static Sha1 Create(string text)
        {
            var utf8Bytes = Encoding.UTF8.GetBytes(text);
            return Create(utf8Bytes);
        }
    }
}