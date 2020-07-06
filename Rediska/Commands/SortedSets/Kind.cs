namespace Rediska.Commands.SortedSets
{
    internal enum Kind : byte
    {
        NegativeInfinity = 0,
        Inclusive = 1,
        Exclusive = 2,
        PositiveInfinity = 3
    }
}