namespace Rediska.Commands.Streams
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public static partial class XINFO
    {
        public sealed class CONSUMERS : Command<IReadOnlyList<Consumer>>
        {
            private static readonly PlainBulkString subName = new PlainBulkString("CONSUMERS");

            private static readonly ListVisitor<Consumer> responseStructure = new ListVisitor<Consumer>(
                ArrayExpectation.Singleton,
                ArrayExpectation2.Singleton.Then(reply => new Consumer(reply))
            );

            private readonly Key key;
            private readonly GroupName groupName;

            public CONSUMERS(Key key, GroupName groupName)
            {
                this.key = key ?? throw new ArgumentNullException(nameof(key));
                this.groupName = groupName;
            }

            public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
            {
                name,
                subName,
                key.ToBulkString(factory),
                groupName.ToBulkString(factory)
            };

            public override Visitor<IReadOnlyList<Consumer>> ResponseStructure => responseStructure;
        }
    }
}