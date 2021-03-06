﻿namespace Rediska.Commands.Streams
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed partial class XPENDING : Command<XPENDING.Summary>
    {
        private static readonly PlainBulkString name = new PlainBulkString("XPENDING");

        private static readonly Visitor<Summary> responseStructure = ArrayExpectation2.Singleton
            .Then(reply => new Summary(reply));

        private readonly Key key;
        private readonly GroupName groupName;

        public XPENDING(Key key, GroupName groupName)
        {
            this.key = key;
            this.groupName = groupName;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory),
            groupName.ToBulkString(factory)
        };

        public override Visitor<Summary> ResponseStructure => responseStructure;
    }
}