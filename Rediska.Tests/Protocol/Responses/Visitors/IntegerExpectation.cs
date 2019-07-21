using System;

namespace Rediska.Tests.Protocol.Responses.Visitors
{
    public sealed class IntegerExpectation : Visitor<long>
    {
        public static IntegerExpectation Singleton { get; } = new IntegerExpectation();

        public override long Visit(Integer integer) => integer.Value;
        public override long Visit(SimpleString simpleString) => throw new ArgumentException("Integer expected");
        public override long Visit(Error error) => throw new ArgumentException("Integer expected");
        public override long Visit(Array array) => throw new ArgumentException("Integer expected");
        public override long Visit(BulkString bulkString) => throw new ArgumentException("Integer expected");
    }
}