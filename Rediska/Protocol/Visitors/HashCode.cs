namespace Rediska.Protocol.Visitors
{
    internal sealed class HashCode : Visitor<int>
    {
        public static HashCode Singleton { get; } = new HashCode();

        public override int Visit(Integer integer) => integer.Value.GetHashCode();

        public override int Visit(SimpleString simpleString) => simpleString.Content.GetHashCode();

        public override int Visit(Error error) => error.Content.GetHashCode();

        public override int Visit(Array array)
        {
            if (array.IsNull)
                return -3;

            if (array.Count == 0)
                return -4;

            var hash = array[0].Accept(this);
            for (var i = 1; i < array.Count; i++)
            {
                var elementHash = array[i].Accept(this);
                hash = (hash * 397) ^ elementHash;
            }

            return hash;
        }

        public override int Visit(BulkString bulkString)
        {
            if (bulkString.IsNull)
                return -1;

            var bytes = bulkString.ToBytes();
            if (bytes.Length == 0)
                return -2;

            var hash = (int) bytes[0];
            for (var i = 1; i < bytes.Length; i++)
            {
                hash = (hash * 31) ^ bytes[i];
            }

            return hash;
        }
    }
}