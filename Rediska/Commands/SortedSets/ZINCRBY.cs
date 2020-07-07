namespace Rediska.Commands.SortedSets
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class ZINCRBY : Command<ZINCRBY.Response>
    {
        private static readonly PlainBulkString name = new PlainBulkString("ZINCRBY");

        private static readonly Visitor<Response> responseStructure = DoubleExpectation.Singleton
            .Then(newScore => new Response(newScore));

        private readonly Key key;
        private readonly double increment;
        private readonly BulkString member;

        public ZINCRBY(Key key, double increment, BulkString member)
        {
            if (double.IsNaN(increment))
                throw new ArgumentException("Must not be NaN", nameof(increment));

            this.key = key;
            this.increment = increment;
            this.member = member;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory),
            factory.Create(increment),
            member
        };

        public override Visitor<Response> ResponseStructure => responseStructure;

        public readonly struct Response
        {
            public Response(double newScore)
            {
                NewScore = newScore;
            }

            public double NewScore { get; }
            public override string ToString() => $"{nameof(NewScore)}: {NewScore}";
        }
    }
}