namespace Rediska.Commands.Streams
{
    using System;
    using Protocol.Visitors;
    using Array = Protocol.Array;

    public sealed partial class XPENDING
    {
        public sealed class EntrySummary
        {
            private readonly Array reply;

            public EntrySummary(Array reply)
            {
                this.reply = reply;
            }

            public Id Id => Id.Parse(
                reply[0].Accept(BulkStringExpectation.Singleton).ToString()
            );

            public ConsumerName Consumer => new ConsumerName(
                reply[1].Accept(BulkStringExpectation.Singleton).ToString()
            );

            public TimeSpan ElapsedSinceLastDelivery => new TimeSpan(
                TimeSpan.TicksPerMillisecond * reply[2].Accept(IntegerExpectation.Singleton)
            );

            public long TimesDelivered => reply[3].Accept(IntegerExpectation.Singleton);
        }
    }
}