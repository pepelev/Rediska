namespace Rediska.Commands.Streams
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed partial class XPENDING
    {
        public sealed partial class Detailed : Command<IReadOnlyList<EntrySummary>>
        {
            private static readonly ListVisitor<EntrySummary> detailedResponseStructure =
                new ListVisitor<EntrySummary>(
                    ArrayExpectation.Singleton,
                    ArrayExpectation2.Singleton.Then(array => new EntrySummary(array))
                );

            private readonly Key key;
            private readonly GroupName groupName;
            private readonly Interval interval;
            private readonly long count;

            public Detailed(
                Key key,
                GroupName groupName,
                Interval interval,
                long count)
            {
                if (count < 1)
                    throw new ArgumentOutOfRangeException(nameof(count), count, "Must be positive");

                this.key = key ?? throw new ArgumentNullException(nameof(key));
                this.groupName = groupName;
                this.interval = interval ?? throw new ArgumentNullException(nameof(interval));
                this.count = count;
            }

            public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
            {
                name,
                key.ToBulkString(factory),
                groupName.ToBulkString(factory),
                interval.StartInclusive == Id.Minimum
                    ? RangeConstants.Minimum
                    : interval.StartInclusive.ToBulkString(factory, Id.Print.Full),
                interval.StartInclusive == Id.Maximum
                    ? RangeConstants.Maximum
                    : interval.EndInclusive.ToBulkString(factory, Id.Print.Full),
                factory.Create(count)
            };

            public override Visitor<IReadOnlyList<EntrySummary>> ResponseStructure => detailedResponseStructure;
        }
    }
}