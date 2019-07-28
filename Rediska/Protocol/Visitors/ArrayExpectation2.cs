using System;

namespace Rediska.Protocol.Visitors
{
    public sealed class ArrayExpectation2 : Visitor<Array>
    {
        public static ArrayExpectation2 Singleton { get; } = new ArrayExpectation2();

        public override Array Visit(Integer integer) => throw new ArgumentException("Array expected");
        public override Array Visit(SimpleString simpleString) => throw new ArgumentException("Array expected");
        public override Array Visit(Error error) => throw new ArgumentException("Array expected");
        public override Array Visit(Array array) => array;
        public override Array Visit(BulkString bulkString) => throw new ArgumentException("Array expected");
    }
}