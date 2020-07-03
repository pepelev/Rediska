namespace Rediska.Commands.Server
{
    using System;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class SWAPDB : Command<None>
    {
        private static readonly PlainBulkString name = new PlainBulkString("SWAPDB");
        private readonly ulong index1;
        private readonly ulong index2;

        public SWAPDB(ulong index1, ulong index2)
        {
            if (index1 > long.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(index1), index1, "Must fit long");

            if (index2 > long.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(index2), index2, "Must fit long");

            this.index1 = index1;
            this.index2 = index2;
        }

        public override DataType Request => new PlainArray(
            name,
            ((long)index1).ToBulkString(),
            ((long)index2).ToBulkString()
        );

        public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
    }
}