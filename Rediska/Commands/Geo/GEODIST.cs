namespace Rediska.Commands.Geo
{
    using Protocol;
    using Protocol.Visitors;

    public sealed class GEODIST : Command<Distance?>
    {
        private static readonly PlainBulkString name = new PlainBulkString("GEODIST");
        private readonly Key key;
        private readonly BulkString member;
        private readonly BulkString anotherMember;
        private readonly Unit unit;

        public GEODIST(Key key, BulkString member, BulkString anotherMember)
            : this(key, member, anotherMember, Unit.Meter)
        {
        }

        public GEODIST(Key key, BulkString member, BulkString anotherMember, Unit unit)
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
            member,
            anotherMember
        );

        public DataType FullRequest => new PlainArray(
            name,
            key.ToBulkString(),
            member,
            anotherMember,
            unit.ToBulkString()
        );

        public override Visitor<Distance?> ResponseStructure => BulkStringExpectation.Singleton
            .Then(
                distance =>
                {
                    if (distance.IsNull)
                        return default(Distance?);

                    var value = double.Parse(distance.ToString());
                    return new Distance(value, unit);
                }
            );
    }
}