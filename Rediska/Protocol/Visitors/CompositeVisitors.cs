namespace Rediska.Protocol.Visitors
{
    using System;
    using System.Collections.Generic;
    using Commands.Keys;
    using Commands.Lists;

    public static class CompositeVisitors
    {
        public static Visitor<ExpireResult> ExpireResult { get; } =
            IntegerExpectation.Singleton.Then(ParseExpireResult);

        public static Visitor<IReadOnlyList<BulkString>> BulkStringList { get; } = new ListVisitor<BulkString>(
            ArrayExpectation.Singleton,
            BulkStringExpectation.Singleton
        );

        public static Visitor<PopResult> Pop => ArrayExpectation2.Singleton.Then(array => new PopResult(array));

        private static ExpireResult ParseExpireResult(long integer)
        {
            switch (integer)
            {
                case 0:
                    return Commands.Keys.ExpireResult.KeyNotExists;
                case 1:
                    return Commands.Keys.ExpireResult.TimeoutSet;
                default:
                    throw new Exception($"Expected 0 or 1, but '{integer}' received");
            }
        }
    }
}