namespace Rediska.Commands.Streams
{
    using System.Collections.Generic;
    using System.Globalization;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed partial class XPENDING
    {
        public sealed class Summary
        {
            private readonly Array reply;

            public Summary(Array reply)
            {
                this.reply = reply;
            }

            public long PendingEntries => reply[0].Accept(IntegerExpectation.Singleton);

            public Id SmallestPendingEntryId => Id.Parse(
                reply[1].Accept(BulkStringExpectation.Singleton).ToString()
            );

            public Id GreatestPendingEntryId => Id.Parse(
                reply[2].Accept(BulkStringExpectation.Singleton).ToString()
            );

            public IReadOnlyList<(ConsumerName Consumer, long PendingMessages)> Consumers
            {
                get
                {
                    return new PrettyReadOnlyList<(ConsumerName Consumer, long PendingMessages)>(
                        new ProjectingReadOnlyList<DataType, (ConsumerName Consumer, long PendingMessages)>(
                            reply[3].Accept(ArrayExpectation.Singleton),
                            pair =>
                            {
                                var content = pair.Accept(ArrayExpectation.Singleton);
                                var consumer = new ConsumerName(
                                    content[0].Accept(BulkStringExpectation.Singleton).ToString()
                                );
                                var pendingMessages = long.Parse(
                                    content[1].Accept(BulkStringExpectation.Singleton).ToString(),
                                    CultureInfo.InvariantCulture
                                );
                                return (consumer, pendingMessages);
                            }
                        )
                    );
                }
            }
        }
    }
}