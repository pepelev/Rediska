namespace Rediska.Commands.Streams
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public static partial class XGROUP
    {
        public sealed class CREATE : Command<None>
        {
            private static readonly PlainBulkString subName = new PlainBulkString("CREATE");
            private static readonly PlainBulkString makeStream = new PlainBulkString("MKSTREAM");
            private readonly Key key;
            private readonly GroupName groupName;
            private readonly Offset offset;
            private readonly Mode mode;

            public CREATE(Key key, GroupName groupName, Offset offset, Mode mode)
            {
                if (mode != Mode.NotCreateStream && mode != Mode.CreateStreamIfNotExists)
                {
                    throw new ArgumentException(
                        $"Must be either NotCreateStream or CreateStreamIfNotExists, but {mode} found",
                        nameof(mode)
                    );
                }

                this.key = key ?? throw new ArgumentNullException(nameof(key));
                this.groupName = groupName;
                this.offset = offset;
                this.mode = mode;
            }

            public override IEnumerable<BulkString> Request(BulkStringFactory factory) => mode == Mode.NotCreateStream
                ? new[]
                {
                    name,
                    subName,
                    key.ToBulkString(factory),
                    groupName.ToBulkString(factory),
                    offset.ToBulkString(factory)
                }
                : new[]
                {
                    name,
                    subName,
                    key.ToBulkString(factory),
                    groupName.ToBulkString(factory),
                    offset.ToBulkString(factory),
                    makeStream
                };

            public override Visitor<None> ResponseStructure => OkExpectation.Singleton;

            public enum Mode : byte
            {
                NotCreateStream = 0,
                CreateStreamIfNotExists = 1
            }
        }
    }
}