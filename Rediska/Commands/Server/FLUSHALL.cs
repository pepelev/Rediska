namespace Rediska.Commands.Server
{
    using System;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed class FLUSHALL : Command<None>
    {
        private static readonly PlainBulkString name = new PlainBulkString("FLUSHALL");
        private static readonly PlainArray syncRequest = new PlainArray(name);
        private static readonly PlainArray asyncRequest = new PlainArray(name, new PlainBulkString("ASYNC"));
        private readonly FlushMode mode;

        public FLUSHALL(FlushMode mode)
        {
            if (mode != FlushMode.Synchronous && mode != FlushMode.Asynchronous)
            {
                throw new ArgumentException(
                    $"Must be either Synchronous or Asynchronous, but {mode} found",
                    nameof(mode)
                );
            }

            this.mode = mode;
        }

        public override DataType Request => mode == FlushMode.Synchronous
            ? syncRequest
            : asyncRequest;

        public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
        public static FLUSHALL Sync { get; } = new FLUSHALL(FlushMode.Synchronous);
        public static FLUSHALL Async { get; } = new FLUSHALL(FlushMode.Asynchronous);
    }
}