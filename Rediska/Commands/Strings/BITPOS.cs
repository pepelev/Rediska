namespace Rediska.Commands.Strings
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class BITPOS : Command<BITPOS.Response>
    {
        private static readonly PlainBulkString name = new PlainBulkString("BITPOS");
        private readonly Key key;
        private readonly bool bit;
        private readonly Index start;
        private readonly Index? end;

        public BITPOS(Key key, bool bit)
            : this(key, bit, Index.Start)
        {
        }

        public BITPOS(Key key, bool bit, Index start)
            : this(key, bit, start, null)
        {
        }

        public BITPOS(Key key, bool bit, Index start, Index? end)
        {
            this.key = key;
            this.bit = bit;
            this.start = start;
            this.end = end;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory)
        {
            return (start, end) switch
            {
                (_, { } endValue) => new[]
                {
                    name,
                    key.ToBulkString(factory),
                    Bit,
                    start.ToBulkString(factory),
                    endValue.ToBulkString(factory)
                },
                (var startValue, null) when startValue == Index.Start => new[]
                {
                    name,
                    key.ToBulkString(factory),
                    Bit
                },
                _ => new[]
                {
                    name,
                    key.ToBulkString(factory),
                    Bit,
                    start.ToBulkString(factory)
                }
            };
        }

        public override Visitor<Response> ResponseStructure => IntegerExpectation.Singleton
            .Then(firstMatchingBitPosition => new Response(bit, end == null, firstMatchingBitPosition));

        private BulkString Bit => bit
            ? BulkStringConstants.One
            : BulkStringConstants.Zero;

        public readonly struct Response
        {
            public Response(bool bit, bool requestRangeIsOpen, long rawResponse)
            {
                Bit = bit;
                RequestRangeIsOpen = requestRangeIsOpen;
                RawResponse = rawResponse;
            }

            public bool Bit { get; }
            public bool RequestRangeIsOpen { get; }
            public long RawResponse { get; }

            public long FirstMatchingBitPosition => Found
                ? RawResponse
                : throw new InvalidOperationException(
                    "Requested bit not found. If you want "
                    + "to get raw response value, access "
                    + nameof(RawResponse) + " property"
                );

            public bool Found => FirstMatchingBitPosition > -1;

            public bool PositionMayBeOutside => RequestRangeIsOpen &&
                                                !Bit &&
                                                Found &&
                                                (FirstMatchingBitPosition & 7) == 0;
        }
    }
}