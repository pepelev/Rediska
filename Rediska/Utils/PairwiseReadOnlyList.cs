namespace Rediska.Utils
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal sealed class PairwiseReadOnlyList<TLeft, TRight> : IReadOnlyList<(TLeft, TRight)>
    {
        private readonly IReadOnlyList<TLeft> left;
        private readonly IReadOnlyList<TRight> right;

        public PairwiseReadOnlyList(IReadOnlyList<TLeft> left, IReadOnlyList<TRight> right)
        {
            if (left.Count != right.Count)
                throw new ArgumentException("Lists must be the same size");

            this.left = left;
            this.right = right;
        }

        public IEnumerator<(TLeft, TRight)> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public int Count => left.Count;
        public (TLeft, TRight) this[int index] => (left[index], right[index]);
    }
}