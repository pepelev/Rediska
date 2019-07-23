using Rediska.Protocol.Responses.Visitors;

namespace Rediska.Protocol.Responses
{
    public abstract class DataType
    {
        public abstract T Accept<T>(Visitor<T> visitor);

        private bool Equals(DataType other)
        {
            var equality = Accept(Equality.Bootstrap.Singleton);
            return other.Accept(equality);
        }

        public override int GetHashCode() => Accept(HashCode.Singleton);
        public static bool operator ==(DataType left, DataType right) => Equals(left, right);
        public static bool operator !=(DataType left, DataType right) => !Equals(left, right);
        public override bool Equals(object obj) => obj is DataType another && Equals(another);
        public override string ToString() => Accept(PrintVisitor.Root);
    }
}