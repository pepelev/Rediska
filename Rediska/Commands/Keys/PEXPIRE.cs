namespace Rediska.Commands.Keys
{
    using System;
    using Protocol;
    using Protocol.Visitors;

    public sealed class PEXPIRE : Command<ExpireResponse>
    {
        private static readonly PlainBulkString name = new PlainBulkString("PEXPIRE");
        private readonly Key key;
        private readonly long milliseconds;

        public PEXPIRE(Key key, long milliseconds)
        {
            if (milliseconds < 0)
                throw new ArgumentOutOfRangeException(nameof(milliseconds), milliseconds, "Must be nonnegative");

            this.key = key;
            this.milliseconds = milliseconds;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString(),
            milliseconds.ToBulkString()
        );

        public override Visitor<ExpireResponse> ResponseStructure => CompositeVisitors.ExpireResult;
    }
}