namespace Rediska.Commands.Auxiliary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Protocol;
    using Protocol.Visitors;

    public sealed class CachingCommand<T> : Command<T>
    {
        private readonly Command<T> command;
        private volatile IReadOnlyList<BulkString> request;
        private volatile Visitor<T> responseStructure;

        public CachingCommand(Command<T> command)
        {
            this.command = command;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory)
        {
            var storedRequest = request;
            if (storedRequest != null)
                return storedRequest;

            var actualRequest = command.Request(factory) switch
            {
                IReadOnlyList<BulkString> list => list,
                { } sequence => sequence.ToList(),
                null => throw new NullReferenceException("Nested command returned null request")
            };

            var oldValue = Interlocked.CompareExchange(ref request, actualRequest, null);
            return oldValue ?? actualRequest;
        }

        public override Visitor<T> ResponseStructure
        {
            get
            {
                var storedResponseStructure = responseStructure;
                if (storedResponseStructure != null)
                    return storedResponseStructure;

                var actualResponseStructure = command.ResponseStructure;
                if (actualResponseStructure == null)
                    throw new NullReferenceException("Nested command returned null response structure");

                var oldValue = Interlocked.CompareExchange(
                    ref responseStructure,
                    actualResponseStructure,
                    null
                );
                return oldValue ?? actualResponseStructure;
            }
        }
    }
}