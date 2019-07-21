using System.Collections.Generic;

namespace Rediska.Tests.Protocol.Responses.Visitors
{
    public sealed class ListVisitor<T> : Visitor<IReadOnlyList<T>>
    {
        private readonly Visitor<IReadOnlyList<DataType>> listVisitor;
        private readonly Visitor<T> itemVisitor;

        public ListVisitor(
            Visitor<IReadOnlyList<DataType>> listVisitor,
            Visitor<T> itemVisitor)
        {
            this.listVisitor = listVisitor;
            this.itemVisitor = itemVisitor;
        }

        public override IReadOnlyList<T> Visit(Integer integer)
        {
            var list = listVisitor.Visit(integer);
            return Project(list);
        }

        public override IReadOnlyList<T> Visit(SimpleString simpleString)
        {
            var list = listVisitor.Visit(simpleString);
            return Project(list);
        }

        public override IReadOnlyList<T> Visit(Error error)
        {
            var list = listVisitor.Visit(error);
            return Project(list);
        }

        public override IReadOnlyList<T> Visit(Array array)
        {
            var list = listVisitor.Visit(array);
            return Project(list);
        }

        public override IReadOnlyList<T> Visit(BulkString bulkString)
        {
            var list = listVisitor.Visit(bulkString);
            return Project(list);
        }

        private IReadOnlyList<T> Project(IReadOnlyList<DataType> list) => new ProjectingReadOnlyList<DataType, T>(
            list,
            item => item.Accept(itemVisitor)
        );
    }
}