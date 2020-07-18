namespace Rediska.Commands.Streams
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;
    using Utils;

    public static partial class XGROUP
    {
        public sealed class SETID : Command<None>
        {
            private static readonly PlainBulkString subName = new PlainBulkString("SETID");
            private readonly Key key;
            private readonly GroupName groupName;
            private readonly Offset offset;

            public SETID(Key key, GroupName groupName, Offset offset)
            {
                this.key = key;
                this.groupName = groupName;
                this.offset = offset;
            }

            public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
            {
                name,
                subName,
                key.ToBulkString(factory),
                groupName.ToBulkString(factory),
                offset.ToBulkString(factory)
            };

            public override Visitor<None> ResponseStructure => OkExpectation.Singleton;
        }
    }
}