namespace Rediska.Commands.Geo
{
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

        public override DataType Request => unit == Unit.Meter
            ? ShortRequest
            : FullRequest;

        public DataType ShortRequest => new PlainArray(
            name,
            key.ToBulkString(),
            member.ToBulkString(),
            anotherMember.ToBulkString()
        );

        public DataType FullRequest => new PlainArray(
            name,
            key.ToBulkString(),
            member.ToBulkString(),
            anotherMember.ToBulkString(),
            unit.ToBulkString()
        );

        public override Visitor<Distance?> ResponseStructure => BulkStringExpectation.Singleton
            .Then(
                distance => distance.ToDoubleOrNull() switch
                {
                    {} value => new Distance(value, unit),
                    null => default(Distance?)
                }
            );
    }
}