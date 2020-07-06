namespace Rediska.Commands.Server
{
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class LASTSAVE : Command<UnixTimestamp>
    {
        private static readonly BulkString[] request = {new PlainBulkString("LASTSAVE")};

        private static readonly Visitor<UnixTimestamp> responseStructure = IntegerExpectation.Singleton
            .Then(unixTime => new UnixTimestamp(unixTime));

        public static LASTSAVE Singleton { get; } = new LASTSAVE();
        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => request;
        public override Visitor<UnixTimestamp> ResponseStructure => responseStructure;
    }
}