namespace Rediska.Commands.Geo
{
    public readonly struct Distance
    {
        public Distance(double value, Unit unit)
        {
            Value = value;
            Unit = unit;
        }

        public double Value { get; }
        public Unit Unit { get; }
        public override string ToString() => $"{Value}{Unit}";
    }
}