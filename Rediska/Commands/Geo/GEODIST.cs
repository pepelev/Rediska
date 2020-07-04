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

        public override IEnumerable<BulkString> Request(BulkStringFactory factory)
        {
            return unit == Unit.Meter
                ? ShortRequest
                : FullRequest;
        }

        public override Visitor<Distance?> ResponseStructure => BulkStringExpectation.Singleton
            .Then(
                distance => distance.ToDoubleOrNull() switch
                {
                    {} value => new Distance(value, unit),
                    null => default(Distance?)
                }
            );

        private BulkString[] ShortRequest => new[]
        {
            name,
            key.ToBulkString(),
            member.ToBulkString(),
            anotherMember.ToBulkString()
        };

        private BulkString[] FullRequest => new[]
        {
            name,
            key.ToBulkString(),
            member.ToBulkString(),
            anotherMember.ToBulkString(),
            unit.ToBulkString()
        };
    }
}