using System;

namespace Rediska.Tests
{
    public struct Magic : IEquatable<Magic>
    {
        public static Magic SimpleString => new Magic('+');
        public static Magic Error => new Magic('-');
        public static Magic Integer => new Magic(':');
        public static Magic BulkString => new Magic('$');
        public static Magic Array => new Magic('*');

        private readonly byte content;

        internal Magic(byte content)
        {
            this.content = content;
        }

        public static bool operator ==(Magic left, Magic right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Magic left, Magic right)
        {
            return !left.Equals(right);
        }

        internal Magic(char content)
            : this((byte)content)
        {
        }

        internal byte ToByte()
        {
            return content;
        }

        public bool Equals(Magic other)
        {
            return content == other.content;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;

            return obj is Magic magic && Equals(magic);
        }

        public override int GetHashCode()
        {
            return content.GetHashCode();
        }

        public override string ToString()
        {
            if (this == SimpleString)
                return nameof(SimpleString);
            if (this == Error)
                return nameof(Error);
            if (this == Integer)
                return nameof(Integer);
            if (this == BulkString)
                return nameof(BulkString);
            if (this == Array)
                return nameof(Array);

            return ((char)content).ToString();
        }
    }
}