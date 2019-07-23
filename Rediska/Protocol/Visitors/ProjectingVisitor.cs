using System;

namespace Rediska.Protocol.Visitors
{
    public sealed class ProjectingVisitor<T, TResult> : Visitor<TResult>
    {
        private readonly Visitor<T> visitor;
        private readonly Func<T, TResult> projection;

        public ProjectingVisitor(Visitor<T> visitor, Func<T, TResult> projection)
        {
            this.visitor = visitor;
            this.projection = projection;
        }

        public override TResult Visit(Integer integer)
        {
            var value = visitor.Visit(integer);
            return projection(value);
        }

        public override TResult Visit(SimpleString simpleString)
        {
            var value = visitor.Visit(simpleString);
            return projection(value);
        }

        public override TResult Visit(Error error)
        {
            var value = visitor.Visit(error);
            return projection(value);
        }

        public override TResult Visit(Array array)
        {
            var value = visitor.Visit(array);
            return projection(value);
        }

        public override TResult Visit(BulkString bulkString)
        {
            var value = visitor.Visit(bulkString);
            return projection(value);
        }
    }
}