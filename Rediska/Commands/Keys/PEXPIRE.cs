namespace Rediska.Commands.Keys
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class PEXPIRE : Command<ExpireResponse>
    {
        private static readonly PlainBulkString name = new PlainBulkString("PEXPIRE");
        private readonly Key key;
        private readonly long milliseconds;

        // todo milliseconds to semantic struct
        public PEXPIRE(Key key, long milliseconds)
        {
            if (milliseconds < 0)
                throw new ArgumentOutOfRangeException(nameof(milliseconds), milliseconds, "Must be nonnegative");

            this.key = key;
            this.milliseconds = milliseconds;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory),
            factory.Create(milliseconds)
        };

        public override Visitor<ExpireResponse> ResponseStructure => CompositeVisitors.ExpireResult;
    }
}