namespace Rediska.Commands.Lists
{
    using System;
    using Protocol;
    using Protocol.Visitors;
    using Array = Protocol.Array;

    public readonly struct PopResult
    {
        private readonly Array reply;

        public PopResult(Array reply)
        {
            if (!reply.IsNull && reply.Count != 2)
            {
                throw new ArgumentException(
                    "Reply must be either null or 2 element array",
                    nameof(reply)
                );
            }

            this.reply = reply;
        }

        public PopStatus Status => reply.IsNull
            ? PopStatus.TimeoutExpired
            : PopStatus.Ok;

        public Key Key => new Key.BulkString(reply[0].Accept(BulkStringExpectation.Singleton));
        public BulkString Value => reply[1].Accept(BulkStringExpectation.Singleton);
    }
}