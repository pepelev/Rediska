namespace Rediska.Commands.Geo
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class GEODIST : Command<Distance?>
    {
        private static readonly PlainBulkString name = new PlainBulkString("GEODIST");
        private readonly Key key;
        private readonly Key member;
        private readonly Key anotherMember;
        private readonly Unit unit;

        public GEODIST(Key key, Key member, Key anotherMember)
            : this(key, member, anotherMember, Unit.Meter)
        {
        }

        public GEODIST(Key key, Key member, Key anotherMember, Unit unit)
        {
            this.key = key;
            this.member = member;
            this.anotherMember = anotherMember;
            this.unit = unit;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => unit == Unit.Meter
            ? ShortRequest(factory)
            : FullRequest(factory);

        public override Visitor<Distance?> ResponseStructure => BulkStringExpectation.Singleton
            .Then(
                distance => distance.ToDoubleOrNull() switch
                {
                    {} value => new Distance(value, unit),
                    null => default(Distance?)
                }
            );

        private BulkString[] ShortRequest(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory),
            member.ToBulkString(factory),
            anotherMember.ToBulkString(factory)
        };

        private BulkString[] FullRequest(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory),
            member.ToBulkString(factory),
            anotherMember.ToBulkString(factory),
            unit.ToBulkString()
        };
    }
}