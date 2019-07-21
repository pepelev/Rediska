using System;

namespace Rediska.Protocol.Responses.Visitors
{
    public sealed class BulkStringExpectation : Visitor<BulkString>
    {
        public static BulkStringExpectation Singleton { get; } = new BulkStringExpectation();

        public override BulkString Visit(Integer integer) => throw new ArgumentException("BulkString expected");
        public override BulkString Visit(SimpleString simpleString) => throw new ArgumentException("BulkString expected");
        public override BulkString Visit(Error error) => throw new ArgumentException("BulkString expected");
        public override BulkString Visit(Array array) => throw new ArgumentException("BulkString expected");
        public override BulkString Visit(BulkString bulkString) => bulkString;
    }
}