namespace Rediska.Commands.Server
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed partial class COMMAND
    {
        public sealed class GETKEYS : Command<IReadOnlyList<Key>>
        {
            private static readonly PlainBulkString subName = new PlainBulkString("GETKEYS");
            private readonly IReadOnlyList<BulkString> command;

            public GETKEYS(params BulkString[] command)
                : this(command as IReadOnlyList<BulkString>)
            {
            }

            public GETKEYS(IReadOnlyList<BulkString> command)
            {
                this.command = command;
            }

            public override DataType Request => new PlainArray(
                new ConcatList<DataType>(
                    new[] {name, subName},
                    command
                )
            );

            public override Visitor<IReadOnlyList<Key>> ResponseStructure => CompositeVisitors.KeyList;
        }
    }
}