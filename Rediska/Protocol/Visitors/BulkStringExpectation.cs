using System;

namespace Rediska.Protocol.Visitors
{
    public sealed class BulkStringExpectation : Expectation<BulkString>
    {
        public static BulkStringExpectation Singleton { get; } = new BulkStringExpectation();
        public override string Message => "BulkString";
        public override BulkString Visit(BulkString bulkString) => bulkString;
    }
}