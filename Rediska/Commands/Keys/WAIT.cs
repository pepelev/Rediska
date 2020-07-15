namespace Rediska.Commands.Keys
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class WAIT : Command<long>
    {
        private static readonly PlainBulkString name = new PlainBulkString("WAIT");
        private readonly long numReplicas;
        private readonly MillisecondsTimeout timeout;

        public WAIT(long numReplicas, MillisecondsTimeout timeout)
        {
            if (numReplicas <= 0)
            {
                var message = numReplicas == 0
                    ? "Zero replicas is meaningless"
                    : "Number of replicas must be positive";
                throw new ArgumentOutOfRangeException(nameof(numReplicas), numReplicas, message);
            }

            this.numReplicas = numReplicas;
            this.timeout = timeout;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            factory.Create(numReplicas),
            timeout.ToBulkString(factory)
        };

        public override Visitor<long> ResponseStructure => IntegerExpectation.Singleton;
    }
}