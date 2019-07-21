using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rediska.Utils
{
    internal sealed class ProjectingCollection<T, TResult> : IReadOnlyCollection<TResult>
    {
        private readonly IReadOnlyCollection<T> collection;
        private readonly Func<T, TResult> projection;

        public ProjectingCollection(IReadOnlyCollection<T> collection, Func<T, TResult> projection)
        {
            this.collection = collection;
            this.projection = projection;
        }

        public IEnumerator<TResult> GetEnumerator() => collection.Select(projection).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public int Count => collection.Count;
    }
}