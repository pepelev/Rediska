namespace Rediska.Commands.Server
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public sealed partial class COMMAND
    {
        public sealed class INFO : Command<IReadOnlyList<CommandDescription>>
        {
            private static readonly PlainBulkString subName = new PlainBulkString("INFO");
            private readonly IReadOnlyList<BulkString> commandNames;

            public INFO(params BulkString[] commandNames)
                : this(commandNames as IReadOnlyList<BulkString>)
            {
            }

            public INFO(IReadOnlyList<BulkString> commandNames)
            {
                if (commandNames.Count < 1)
                    throw new ArgumentException("Must contain elements", nameof(commandNames));

                this.commandNames = commandNames;
            }

            public override DataType Request => new PlainArray(
                new ConcatList<DataType>(
                    new[] {name, subName},
                    commandNames
                )
            );

            public override Visitor<IReadOnlyList<CommandDescription>> ResponseStructure => responseStructure;
        }
    }
}