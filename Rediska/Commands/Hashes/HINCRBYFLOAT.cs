namespace Rediska.Commands.Hashes
{
    using System;
    using Protocol;
    using Protocol.Visitors;

    public sealed class HINCRBYFLOAT : Command<double>
    {
        private static readonly PlainBulkString name = new PlainBulkString("HINCRBYFLOAT");
        private readonly Key key;
        private readonly Key field;
        private readonly double increment;

        public HINCRBYFLOAT(Key key, Key field, double increment)
        {
            if (double.IsInfinity(increment) || double.IsNaN(increment))
                throw new ArgumentException($"Must be regular number, but {increment} found", nameof(increment));

            this.key = key;
            this.field = field;
            this.increment = increment;
        }

        public override DataType Request => new PlainArray(
            name,
            key.ToBulkString(),
            field.ToBulkString(),
            increment.ToBulkString()
        );

        public override Visitor<double> ResponseStructure => CompositeVisitors.Double;
    }
}