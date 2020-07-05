namespace Rediska.Commands.Server
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class TIME : Command<DateTime>
    {
        private const int ticksPerMicrosecond = 10;
        private static readonly BulkString[] request = {new PlainBulkString("TIME")};

        private static readonly Visitor<DateTime> responseStructure = CompositeVisitors.IntegerList
            .Then(
                response =>
                {
                    if (response.Count != 2)
                        throw new ArgumentException("Must contain 2 elements", nameof(response));

                    var unixTime = response[0];
                    var microseconds = response[1];
                    var unixTimestamp = new UnixTimestamp(unixTime);
                    return unixTimestamp.ToDateTime(DateTimeKind.Unspecified) + new TimeSpan(ticksPerMicrosecond * microseconds);
                }
            );

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => request;
        public override Visitor<DateTime> ResponseStructure => responseStructure;
    }
}