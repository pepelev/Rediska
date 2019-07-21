using System;
using System.Collections.Generic;

namespace Rediska.Tests.Protocol.Responses.Visitors
{
    public sealed class ArrayExpectation : Visitor<IReadOnlyList<DataType>>
    {
        public static ArrayExpectation Singleton { get; } = new ArrayExpectation();

        public override IReadOnlyList<DataType> Visit(Integer integer) => throw new ArgumentException("Array expected");
        public override IReadOnlyList<DataType> Visit(SimpleString simpleString) => throw new ArgumentException("Array expected");
        public override IReadOnlyList<DataType> Visit(Error error) => throw new ArgumentException("Array expected");
        public override IReadOnlyList<DataType> Visit(Array array) => array;
        public override IReadOnlyList<DataType> Visit(BulkString bulkString) => throw new ArgumentException("Array expected");
    }
}