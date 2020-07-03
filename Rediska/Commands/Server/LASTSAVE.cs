namespace Rediska.Commands.Server
{
    using Protocol;
    using Protocol.Visitors;

    public sealed class LASTSAVE : Command<UnixTimestamp>
    {
        private static readonly PlainArray request = new PlainArray(new PlainBulkString("LASTSAVE"));

        private static readonly Visitor<UnixTimestamp> responseStructure = IntegerExpectation.Singleton
            .Then(unixTime => new UnixTimestamp(unixTime));

        public override DataType Request => request;
        public override Visitor<UnixTimestamp> ResponseStructure => responseStructure;
        public static LASTSAVE Singleton { get; } = new LASTSAVE();
    }
}