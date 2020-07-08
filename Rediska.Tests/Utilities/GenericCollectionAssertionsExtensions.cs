namespace Rediska.Tests.Utilities
{
    using System.Linq;
    using FluentAssertions.Collections;

    public static class GenericCollectionAssertionsExtensions
    {
        public static void Be<T>(
            this GenericCollectionAssertions<T> assertions,
            params T[] elements)
        {
            assertions.BeEquivalentTo(elements.AsEnumerable());
        }
    }
}