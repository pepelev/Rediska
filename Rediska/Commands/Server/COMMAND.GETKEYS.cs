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
            private readonly IReadOnlyList<BulkString> commands;
            private readonly PlainBulkString[] prefix = {name, subName};

            public GETKEYS(params BulkString[] command)
                : this(command as IReadOnlyList<BulkString>)
            {
            }

            public GETKEYS(IReadOnlyList<BulkString> commands)
            {
                this.commands = commands;
            }

            public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new ConcatList<BulkString>(
                prefix,
                commands
            );

            public override Visitor<IReadOnlyList<Key>> ResponseStructure => CompositeVisitors.KeyList;
        }
    }
}