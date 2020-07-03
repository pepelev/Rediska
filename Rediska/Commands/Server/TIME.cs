namespace Rediska.Commands.Server
{
    using System;
    using Protocol;
    using Protocol.Visitors;

    public sealed class TIME : Command<DateTime>
    {
        private const int ticksPerMicrosecond = 10;
        private static readonly PlainArray request = new PlainArray(new PlainBulkString("TIME"));

        private static readonly Visitor<DateTime> responseStructure = ArrayExpectation.Singleton
            .Then(
                response =>
                {
                    if (response.Count != 2)
                        throw new ArgumentException("Must contain 2 elements", nameof(response));

                    var unixTime = response[0].Accept(IntegerExpectation.Singleton);
                    var microseconds = response[1].Accept(IntegerExpectation.Singleton);
                    var unixTimestamp = new UnixTimestamp(unixTime);
                    return unixTimestamp.ToDateTime(DateTimeKind.Unspecified) + new TimeSpan(ticksPerMicrosecond * microseconds);
                }
            );

        public override DataType Request => request;
        public override Visitor<DateTime> ResponseStructure => responseStructure;
    }
}