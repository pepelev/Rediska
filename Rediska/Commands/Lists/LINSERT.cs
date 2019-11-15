namespace Rediska.Commands.Lists
{
    using System;
    using System.Globalization;
    using Protocol;
    using Protocol.Visitors;

    public sealed class LINSERT : Command<LINSERT.Result>
    {
        public enum Status : byte
        {
            PivotNotFound,
            ListIsEmpty,
            Ok
        }

        public enum Where : byte
        {
            BEFORE,
            AFTER
        }

        private static readonly PlainBulkString name = new PlainBulkString("LINSERT");
        private static readonly PlainBulkString before = new PlainBulkString("BEFORE");
        private static readonly PlainBulkString after = new PlainBulkString("AFTER");
        private readonly Key key;
        private readonly Where where;
        private readonly BulkString pivot;
        private readonly BulkString element;

        public LINSERT(Key key, Where where, BulkString pivot, BulkString element)
        {
            this.key = key;
            this.where = where;
            this.pivot = pivot;
            this.element = element;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString(),
            where == Where.BEFORE
                ? before
                : after,
            pivot,
            element
        );

        public override Visitor<Result> ResponseStructure =>
            IntegerExpectation.Singleton.Then(reply => new Result(reply));

        public readonly struct Result
        {
            public Result(long reply)
            {
                if (reply < -1)
                {
                    throw new ArgumentOutOfRangeException(nameof(reply), "Reply must be -1 or greater");
                }

                Reply = reply;
            }

            public Status Status
            {
                get
                {
                    switch (Reply)
                    {
                        case -1:
                            return Status.PivotNotFound;
                        case 0:
                            return Status.ListIsEmpty;
                        default:
                            return Status.Ok;
                    }
                }
            }

            public long ListLength => Status == Status.Ok || Status == Status.ListIsEmpty
                ? Reply
                : throw new InvalidOperationException("Pivot not found");

            public long Reply { get; }
            public override string ToString() => Reply.ToString(CultureInfo.InvariantCulture);
        }
    }
}