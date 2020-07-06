namespace Rediska.Commands.Hashes
{
    using System;
    using System.Collections.Generic;
    using Protocol;
    using Protocol.Visitors;

    public sealed class HINCRBYFLOAT : Command<double>
    {
        private static readonly PlainBulkString name = new PlainBulkString("HINCRBYFLOAT");
        private readonly Key key;
        private readonly BulkString field;
        private readonly double increment;

        public HINCRBYFLOAT(Key key, BulkString field, double increment)
        {
            if (double.IsInfinity(increment) || double.IsNaN(increment))
                throw new ArgumentException($"Must be regular number, but {increment} found", nameof(increment));

            this.key = key;
            this.field = field;
            this.increment = increment;
        }

        public override IEnumerable<BulkString> Request(BulkStringFactory factory) => new[]
        {
            name,
            key.ToBulkString(factory),
            field,
            factory.Create(increment)
        };

        public override Visitor<double> ResponseStructure => CompositeVisitors.Double;
    }
}