using System;

namespace Rediska.Protocol.Visitors
{
    public static class VisitorExtensions
    {
        public static Visitor<TResult> Then<T, TResult>(this Visitor<T> visitor, Func<T, TResult> projection) =>
            new ProjectingVisitor<T, TResult>(
                visitor,
                projection
            );
    }
}