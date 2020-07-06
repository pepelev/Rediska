namespace Rediska.Protocol.Visitors
{
    public sealed class NullableIntegerExpectation : Expectation<long?>
    {
        public static NullableIntegerExpectation Singleton { get; } = new NullableIntegerExpectation();
        public override string Message => "Integer or null";
        public override long? Visit(Integer integer) => integer.Value;

        public override long? Visit(Array array) => array.IsNull
            ? default(long?)
            : throw Exception(array);

        public override long? Visit(BulkString bulkString) => bulkString.IsNull
            ? default(long?)
            : throw Exception(bulkString);
    }
}