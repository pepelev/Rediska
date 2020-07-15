namespace Rediska.Commands.Auxiliary
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class CustomResponseHandling<TInner, TResult> : Command<TResult>
    {
        private readonly Command<TInner> command;

        public CustomResponseHandling(Command<TInner> command, Visitor<TResult> responseStructure)
        {
            this.command = command ?? throw new ArgumentNullException(nameof(command));
            ResponseStructure = responseStructure ?? throw new ArgumentNullException(nameof(responseStructure));
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => command.Request(factory);
        public override Visitor<TResult> ResponseStructure { get; }
    }
}