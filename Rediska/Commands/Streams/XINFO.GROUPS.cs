namespace Rediska.Commands.Streams
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public static partial class XINFO
    {
        public sealed class GROUPS : Command<IReadOnlyList<Group>>
        {
            private static readonly PlainBulkString subName = new PlainBulkString("GROUPS");

            private static readonly ListVisitor<Group> responseStructure = new ListVisitor<Group>(
                ArrayExpectation.Singleton,
                ArrayExpectation2.Singleton.Then(reply => new Group(reply))
            );

            private readonly Key key;

            public GROUPS(Key key)
            {
                this.key = key ?? throw new ArgumentNullException(nameof(key));
            }

            public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
            {
                name,
                subName,
                key.ToBulkString(factory)
            };

            public override Visitor<IReadOnlyList<Group>> ResponseStructure => responseStructure;
        }
    }
}